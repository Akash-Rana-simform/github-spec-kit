# Data Model: Bookmark Organizer CLI

**Phase**: 1 (Design & Contracts)  
**Created**: 2026-04-28  
**Purpose**: Define data structures and their relationships

## Core Entities

### Bookmark

Represents a saved web URL with associated metadata.

**Attributes**:
- `url` (string, required, unique): The web address to bookmark
  - Must be valid URL format (http:// or https://)
  - Serves as primary identifier (no duplicate URLs allowed)
  - Validation: URL format check on add/import

- `title` (string, optional): Display name for the bookmark
  - Defaults to URL if not provided
  - Can be manually edited after creation
  - Displayed in list and search results

- `notes` (string, optional): User-added description or context
  - Freeform text field
  - Supports multiline text
  - Searchable content

- `tags` (list of strings, optional): Labels for categorization
  - Zero or more tags per bookmark
  - Tags are normalized to lowercase for consistency
  - Supports hierarchical tags with `/` separator (e.g., `programming/python`)
  - Tags are used as primary filter mechanism

- `created` (ISO 8601 timestamp, required, auto-generated): When bookmark was first added
  - Format: `YYYY-MM-DDTHH:MM:SSZ`
  - Set automatically on creation
  - Immutable after creation

- `modified` (ISO 8601 timestamp, required, auto-generated): When bookmark was last updated
  - Format: `YYYY-MM-DDTHH:MM:SSZ`
  - Updated automatically when title, notes, or tags change
  - Initialized to `created` value on first add

**Validation Rules**:
1. URL must be non-empty and match basic URL pattern
2. URL must be unique within bookmark collection
3. Title max length: 500 characters (soft limit, warn if exceeded)
4. Notes max length: 5000 characters (soft limit, warn if exceeded)
5. Tag names must contain only alphanumeric, spaces, hyphens, underscores, or `/`
6. Individual tag max length: 100 characters

**Example**:
```json
{
  "url": "https://docs.python.org/3/library/json.html",
  "title": "Python JSON Documentation",
  "notes": "Reference for json.dump() and json.load() methods",
  "tags": ["python", "reference", "programming/python"],
  "created": "2026-04-28T18:00:00Z",
  "modified": "2026-04-28T18:30:00Z"
}
```

---

### Tag

Tags are not stored as separate entities but are derived from bookmarks. This section documents tag-related concepts and operations.

**Characteristics**:
- **Implicit Entity**: Tags exist only as attributes of bookmarks
- **Case-Insensitive**: "Python", "python", "PYTHON" are treated as the same tag
- **Many-to-Many**: One bookmark can have many tags; one tag can apply to many bookmarks
- **Hierarchical Support**: Tags can represent hierarchy using `/` (e.g., `tech/python/django`)
- **Derived Operations**: 
  - List all unique tags across all bookmarks
  - Count bookmark usage per tag
  - Filter bookmarks by tag(s)

**Tag Normalization**:
- Convert to lowercase: "Python" → "python"
- Trim whitespace: " python  " → "python"
- Collapse multiple spaces: "python   tutorial" → "python tutorial"
- Reject invalid characters (anything other than alphanumeric, space, `-`, `_`, `/`)

**Example Tag Queries**:
```python
# Get all unique tags from bookmark collection
all_tags = set(tag for bookmark in bookmarks for tag in bookmark.tags)

# Count bookmarks per tag
tag_counts = Counter(tag for bookmark in bookmarks for tag in bookmark.tags)

# Find bookmarks with specific tag
python_bookmarks = [b for b in bookmarks if "python" in b.tags]
```

---

## Storage Model

### File Structure

**Primary Storage**: `~/.bookmarks/bookmarks.json`

**File Format**:
```json
{
  "metadata": {
    "version": "1.0",
    "last_modified": "2026-04-28T18:30:00Z",
    "bookmark_count": 42
  },
  "bookmarks": [
    {
      "url": "https://example.com",
      "title": "Example",
      "notes": "",
      "tags": ["example"],
      "created": "2026-04-28T18:00:00Z",
      "modified": "2026-04-28T18:00:00Z"
    }
  ]
}
```

**Metadata Section**:
- `version` (string): Schema version for future migrations
- `last_modified` (timestamp): Last time file was written
- `bookmark_count` (integer): Total number of bookmarks (for quick stats)

**Bookmarks Section**:
- Array of Bookmark objects
- No specific ordering required (display order determined at runtime)
- Each bookmark is a complete, self-contained object

---

### File Operations

**Read Operation** (`load_bookmarks`):
1. Check if `~/.bookmarks/bookmarks.json` exists
2. If not exists, return empty collection (initialize on first add)
3. If exists, read file content
4. Parse JSON (handle parse errors gracefully)
5. Validate schema (check required fields)
6. Return list of Bookmark objects

**Write Operation** (`save_bookmarks`):
1. Serialize bookmarks to JSON with `indent=2`
2. Update metadata (last_modified, bookmark_count)
3. Write to temporary file `~/.bookmarks/bookmarks.json.tmp`
4. Verify write successful and file not empty
5. Atomic rename: `os.replace(tmp, target)` (ensures atomicity)
6. If write fails, leave original file untouched

**Backup Operation** (before destructive changes):
1. If `bookmarks.json` exists, copy to `bookmarks.json.backup`
2. Keep only most recent backup (don't accumulate multiple)
3. Restore from backup if user requests via `bookmark restore` command

---

## Relationships

### Bookmark ↔ Tags

**Relationship Type**: Many-to-Many (implicit through list attribute)

**Navigation**:
- **Bookmark → Tags**: Direct access via `bookmark.tags` list
- **Tag → Bookmarks**: Filter operation across all bookmarks

**Operations**:
- **Add tag to bookmark**: Append to `tags` list if not already present
- **Remove tag from bookmark**: Remove from `tags` list
- **Find bookmarks by tag**: Filter where tag in `bookmark.tags`
- **Find bookmarks by multiple tags**: Filter where all tags in `bookmark.tags` (AND logic)

**Example**:
```python
# Add tag
if "python" not in bookmark.tags:
    bookmark.tags.append("python")

# Remove tag
bookmark.tags = [t for t in bookmark.tags if t != "obsolete"]

# Find by single tag
results = [b for b in bookmarks if "python" in b.tags]

# Find by multiple tags (AND)
results = [b for b in bookmarks if all(tag in b.tags for tag in ["python", "tutorial"])]
```

---

## State Transitions

### Bookmark Lifecycle

```
[New] --add--> [Active] --edit--> [Active] --delete--> [Removed]
                  |
                  +--export--> [Exported]
```

**States**:
- **New**: Bookmark defined but not yet saved (transient state during add command)
- **Active**: Bookmark persisted in storage, available for search and listing
- **Removed**: Bookmark deleted from storage (no soft delete in v1)
- **Exported**: Bookmark serialized to external format (copy of Active state)

**Transitions**:
- **add**: New → Active (validate URL, create timestamps, save to file)
- **edit**: Active → Active (update fields, update modified timestamp, save to file)
- **delete**: Active → Removed (remove from list, save to file)
- **export**: Active → Exported (serialize to JSON/HTML, original remains Active)
- **import**: New → Active (parse external format, deduplicate, save to file)

---

## Data Validation

### URL Validation

```python
import re
from urllib.parse import urlparse

def is_valid_url(url: str) -> bool:
    """Validate URL format"""
    try:
        result = urlparse(url)
        return all([result.scheme in ['http', 'https'], result.netloc])
    except:
        return False
```

### Tag Validation

```python
import re

def is_valid_tag(tag: str) -> bool:
    """Validate tag format"""
    # Allow alphanumeric, spaces, hyphens, underscores, forward slash
    pattern = r'^[\w\s\-/]+$'
    return bool(re.match(pattern, tag)) and len(tag) <= 100
```

### Duplicate Detection

```python
def find_duplicate_url(url: str, bookmarks: list) -> Optional[Bookmark]:
    """Check if URL already exists"""
    return next((b for b in bookmarks if b.url == url), None)
```

---

## Data Access Patterns

### Primary Access Patterns

1. **List All Bookmarks**
   - Load entire file
   - Return all bookmarks
   - Optional: sort by created date, modified date, or title
   - Performance: O(n) where n = number of bookmarks

2. **Search by Text**
   - Load entire file
   - Filter where query text appears in URL, title, or notes (case-insensitive)
   - Performance: O(n × m) where m = average text length

3. **Filter by Tag(s)**
   - Load entire file
   - Filter where all specified tags present in bookmark.tags
   - Performance: O(n × t) where t = number of tags to match

4. **Add Bookmark**
   - Load entire file
   - Check for duplicate URL
   - Append new bookmark
   - Save entire file
   - Performance: O(n)

5. **Edit Bookmark**
   - Load entire file
   - Find bookmark by URL
   - Update fields
   - Save entire file
   - Performance: O(n)

6. **Delete Bookmark**
   - Load entire file
   - Remove bookmark by URL
   - Save entire file
   - Performance: O(n)

### Performance Characteristics

- **File I/O**: ~5-10ms to read 10k bookmarks (~5MB file)
- **JSON Parsing**: ~10-20ms to parse 10k bookmarks
- **Search**: ~10-50ms to search 10k bookmarks with text matching
- **Write**: ~5-10ms to write file atomically

All operations are well within acceptable performance limits for up to 50k bookmarks.
