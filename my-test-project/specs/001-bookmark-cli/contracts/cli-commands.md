# CLI Command Interface Contracts

**Phase**: 1 (Design & Contracts)  
**Created**: 2026-04-28  
**Purpose**: Define command-line interface contracts for bookmark organizer CLI

## Overview

This document defines the complete command-line interface for the bookmark organizer tool. All commands follow the pattern `bookmark <command> [arguments] [options]`.

---

## Command Group: `bookmark`

Root command group for all bookmark operations.

### Global Options

Available for all subcommands:

- `--help`: Show help message and exit
- `--version`: Show version and exit
- `--verbose`, `-v`: Enable verbose output (show debug information)
- `--quiet`, `-q`: Minimize output (errors only)

---

## Commands

### `bookmark add`

Add a new bookmark to the collection.

**Syntax**:
```bash
bookmark add <url> [options]
```

**Arguments**:
- `url` (required): The URL to bookmark

**Options**:
- `--title TEXT`, `-t TEXT`: Set bookmark title (default: use URL)
- `--note TEXT`, `-n TEXT`: Add notes/description
- `--tag TAG`, `-g TAG`: Add tag (can be specified multiple times)
- `--json`: Output result as JSON

**Behavior**:
- Validates URL format before saving
- Checks for duplicate URL
  - If duplicate exists, prompts: "URL already exists. Update it? [y/N]"
  - If user confirms, updates existing bookmark
  - If user declines, exits without changes
- Creates bookmark with current timestamp
- Saves to storage file atomically

**Output** (human-readable):
```
✓ Bookmark added: <title>
  URL: <url>
  Tags: <tag1>, <tag2>
```

**Output** (JSON format with `--json`):
```json
{
  "status": "success",
  "action": "added",
  "bookmark": {
    "url": "https://example.com",
    "title": "Example",
    "tags": ["example"],
    "created": "2026-04-28T18:00:00Z"
  }
}
```

**Exit Codes**:
- `0`: Success
- `1`: Invalid URL or user declined duplicate update
- `2`: File operation error

**Examples**:
```bash
# Simple add
bookmark add https://example.com

# Add with metadata
bookmark add https://example.com --title "Example Site" --note "Useful resource" --tag python --tag tutorial

# Add and output JSON
bookmark add https://example.com --json
```

---

### `bookmark list`

List all bookmarks or filter by criteria.

**Syntax**:
```bash
bookmark list [options]
```

**Options**:
- `--tag TAG`, `-g TAG`: Filter by tag (can be specified multiple times for AND logic)
- `--sort {created,modified,title}`, `-s {created,modified,title}`: Sort order (default: created)
- `--reverse`, `-r`: Reverse sort order
- `--limit N`, `-l N`: Show only first N results
- `--json`: Output as JSON array

**Behavior**:
- Loads all bookmarks from storage
- Applies tag filters if specified (AND logic for multiple tags)
- Sorts by specified field
- Limits results if requested
- Displays in human-readable format or JSON

**Output** (human-readable):
```
📚 42 bookmarks

Example Site [example]
  https://example.com
  Added: 2026-04-28, Modified: 2026-04-28
  Note: Useful resource

Python Docs [python, reference, programming/python]
  https://docs.python.org
  Added: 2026-04-27
  
---
Showing 2 of 42 bookmarks
```

**Output** (JSON format with `--json`):
```json
{
  "count": 42,
  "bookmarks": [
    {
      "url": "https://example.com",
      "title": "Example Site",
      "notes": "Useful resource",
      "tags": ["example"],
      "created": "2026-04-28T18:00:00Z",
      "modified": "2026-04-28T18:00:00Z"
    }
  ]
}
```

**Exit Codes**:
- `0`: Success (even if no results)
- `2`: File operation error

**Examples**:
```bash
# List all
bookmark list

# Filter by tag
bookmark list --tag python

# Multiple tags (AND logic)
bookmark list --tag python --tag tutorial

# Sort and limit
bookmark list --sort title --limit 10

# JSON output
bookmark list --json
```

---

### `bookmark search`

Search bookmarks by text query.

**Syntax**:
```bash
bookmark search <query> [options]
```

**Arguments**:
- `query` (required): Text to search for (searches URL, title, and notes)

**Options**:
- `--tag TAG`, `-g TAG`: Additional tag filter (can be specified multiple times)
- `--sort {created,modified,title}`, `-s {created,modified,title}`: Sort order
- `--json`: Output as JSON array

**Behavior**:
- Performs case-insensitive substring search in URL, title, and notes
- Applies tag filters if specified
- Returns matching bookmarks sorted by specified order

**Output**: Same format as `bookmark list`

**Exit Codes**:
- `0`: Success (even if no results)
- `2`: File operation error

**Examples**:
```bash
# Simple search
bookmark search python

# Search with tag filter
bookmark search "web scraping" --tag python

# Search for URL pattern
bookmark search example.com

# JSON output
bookmark search tutorial --json
```

---

### `bookmark edit`

Edit an existing bookmark's metadata.

**Syntax**:
```bash
bookmark edit <url> [options]
```

**Arguments**:
- `url` (required): URL of bookmark to edit (exact match)

**Options**:
- `--title TEXT`, `-t TEXT`: Update title
- `--note TEXT`, `-n TEXT`: Update notes (replaces existing notes)
- `--add-tag TAG`: Add a tag
- `--remove-tag TAG`: Remove a tag
- `--json`: Output result as JSON

**Behavior**:
- Finds bookmark by exact URL match
- Updates specified fields only
- Updates `modified` timestamp
- Saves changes atomically

**Output** (human-readable):
```
✓ Bookmark updated: <title>
  URL: <url>
  Changes: title, tags
```

**Output** (JSON format with `--json`):
```json
{
  "status": "success",
  "action": "updated",
  "bookmark": {
    "url": "https://example.com",
    "title": "New Title",
    "modified": "2026-04-28T19:00:00Z"
  }
}
```

**Exit Codes**:
- `0`: Success
- `1`: Bookmark not found
- `2`: File operation error

**Examples**:
```bash
# Update title
bookmark edit https://example.com --title "New Title"

# Add tags
bookmark edit https://example.com --add-tag python --add-tag tutorial

# Remove tag
bookmark edit https://example.com --remove-tag obsolete

# Update multiple fields
bookmark edit https://example.com --title "New Title" --note "Updated description" --add-tag new
```

---

### `bookmark delete`

Delete a bookmark from the collection.

**Syntax**:
```bash
bookmark delete <url> [options]
```

**Arguments**:
- `url` (required): URL of bookmark to delete (exact match)

**Options**:
- `--yes`, `-y`: Skip confirmation prompt
- `--json`: Output result as JSON

**Behavior**:
- Finds bookmark by exact URL match
- Prompts for confirmation unless `--yes` specified
- Removes bookmark from collection
- Saves changes atomically

**Output** (human-readable):
```
Delete bookmark "Example Site" (https://example.com)? [y/N]: y
✓ Bookmark deleted
```

**Output** (JSON format with `--json`):
```json
{
  "status": "success",
  "action": "deleted",
  "url": "https://example.com"
}
```

**Exit Codes**:
- `0`: Success
- `1`: Bookmark not found or user declined
- `2`: File operation error

**Examples**:
```bash
# Delete with confirmation
bookmark delete https://example.com

# Delete without confirmation
bookmark delete https://example.com --yes

# JSON output
bookmark delete https://example.com --yes --json
```

---

### `bookmark tags`

Manage and view tags.

**Subcommands**:
- `bookmark tags list`: List all tags
- `bookmark tags rename`: Rename a tag across all bookmarks

---

#### `bookmark tags list`

List all unique tags with usage counts.

**Syntax**:
```bash
bookmark tags list [options]
```

**Options**:
- `--sort {name,count}`, `-s {name,count}`: Sort by tag name or usage count (default: name)
- `--reverse`, `-r`: Reverse sort order
- `--json`: Output as JSON

**Output** (human-readable):
```
📑 Tags (15 total)

python (12)
tutorial (8)
programming/python (5)
web (4)
reference (3)
...
```

**Output** (JSON format with `--json`):
```json
{
  "count": 15,
  "tags": [
    {"name": "python", "count": 12},
    {"name": "tutorial", "count": 8}
  ]
}
```

**Exit Codes**:
- `0`: Success
- `2`: File operation error

**Examples**:
```bash
# List all tags
bookmark tags list

# Sort by usage count
bookmark tags list --sort count --reverse

# JSON output
bookmark tags list --json
```

---

#### `bookmark tags rename`

Rename a tag across all bookmarks.

**Syntax**:
```bash
bookmark tags rename <old_tag> <new_tag> [options]
```

**Arguments**:
- `old_tag` (required): Current tag name
- `new_tag` (required): New tag name

**Options**:
- `--yes`, `-y`: Skip confirmation prompt
- `--json`: Output result as JSON

**Behavior**:
- Finds all bookmarks with `old_tag`
- Replaces with `new_tag`
- Shows count of affected bookmarks
- Prompts for confirmation unless `--yes` specified

**Output** (human-readable):
```
Rename tag "python" to "python3" (12 bookmarks affected)? [y/N]: y
✓ Renamed tag in 12 bookmarks
```

**Exit Codes**:
- `0`: Success
- `1`: Tag not found or user declined
- `2`: File operation error

**Examples**:
```bash
# Rename with confirmation
bookmark tags rename python python3

# Rename without confirmation
bookmark tags rename python python3 --yes
```

---

### `bookmark import`

Import bookmarks from external file.

**Syntax**:
```bash
bookmark import <file> [options]
```

**Arguments**:
- `file` (required): Path to import file (HTML or JSON)

**Options**:
- `--format {html,json}`: File format (auto-detected if not specified)
- `--dry-run`: Show what would be imported without actually importing
- `--skip-duplicates`: Skip duplicates silently (default: prompt for each)
- `--merge-duplicates`: Merge tags from duplicates (default: prompt for each)
- `--json`: Output result as JSON

**Behavior**:
- Detects file format from extension or content (`.html` → HTML, `.json` → JSON)
- Parses file and extracts bookmarks
- For each bookmark:
  - If URL doesn't exist: add bookmark
  - If URL exists and `--skip-duplicates`: skip
  - If URL exists and `--merge-duplicates`: merge tags from both
  - If URL exists and neither flag: prompt user (skip/merge/replace)
- Shows summary of results

**Output** (human-readable):
```
Importing from bookmarks.html...
✓ Imported 42 bookmarks
  Skipped 3 duplicates
  Merged 2 duplicates
```

**Output** (JSON format with `--json`):
```json
{
  "status": "success",
  "action": "imported",
  "added": 42,
  "skipped": 3,
  "merged": 2,
  "total": 47
}
```

**Exit Codes**:
- `0`: Success (partial import counts as success)
- `1`: Invalid file format
- `2`: File operation error

**Examples**:
```bash
# Import HTML
bookmark import bookmarks.html

# Import with duplicate handling
bookmark import bookmarks.html --skip-duplicates

# Dry run to preview
bookmark import bookmarks.html --dry-run

# Import JSON
bookmark import bookmarks.json --format json
```

---

### `bookmark export`

Export bookmarks to external file.

**Syntax**:
```bash
bookmark export <file> [options]
```

**Arguments**:
- `file` (required): Output file path

**Options**:
- `--format {html,json}`: Export format (default: inferred from file extension)
- `--tag TAG`, `-g TAG`: Export only bookmarks with specified tag(s)
- `--json-output`: Output result summary as JSON (separate from export format)

**Behavior**:
- Loads bookmarks (filtered by tags if specified)
- Serializes to requested format
- Writes to output file
- HTML format: Netscape Bookmark File Format (browser-compatible)
- JSON format: Same schema as internal storage

**Output** (human-readable):
```
✓ Exported 42 bookmarks to bookmarks.html
```

**Output** (with `--json-output`):
```json
{
  "status": "success",
  "action": "exported",
  "count": 42,
  "file": "bookmarks.html",
  "format": "html"
}
```

**Exit Codes**:
- `0`: Success
- `1`: No bookmarks match filter
- `2`: File operation error

**Examples**:
```bash
# Export to HTML
bookmark export bookmarks.html

# Export to JSON
bookmark export backup.json --format json

# Export specific tags
bookmark export python-links.html --tag python

# Multiple tag filter
bookmark export web-dev.html --tag web --tag tutorial
```

---

## Error Handling

### Common Error Messages

**Invalid URL**:
```
Error: Invalid URL format: '<url>'
Example: bookmark add https://example.com
```

**Bookmark Not Found**:
```
Error: No bookmark found with URL: <url>
```

**File Permission Error**:
```
Error: Cannot write to ~/.bookmarks/bookmarks.json
Check file permissions and disk space.
```

**Corrupted Data File**:
```
Error: Bookmark data file is corrupted
Would you like to restore from backup? [y/N]:
```

**Import Parse Error**:
```
Error: Cannot parse import file: <file>
Format: <html|json>
Issue: <specific parse error>
```

---

## Exit Codes

- `0`: Success
- `1`: User error (invalid input, not found, user declined action)
- `2`: System error (file I/O, permission denied, corrupted data)

---

## Version Information

**Format**:
```
bookmark, version 1.0.0
Bookmark organizer CLI tool
```

**Access**:
```bash
bookmark --version
```

---

## Configuration (Future Enhancement)

Future versions may support configuration file at `~/.bookmarks/config.json`:

```json
{
  "storage_path": "~/.bookmarks/bookmarks.json",
  "auto_backup": true,
  "default_sort": "created",
  "color_output": true
}
```

Not implemented in v1.
