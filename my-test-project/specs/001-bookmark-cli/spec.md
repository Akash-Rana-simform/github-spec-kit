# Feature Specification: Bookmark Organizer CLI

**Feature Branch**: `001-bookmark-cli`  
**Created**: 2026-04-28  
**Status**: Draft  
**Input**: User description: "a CLI tool to organize bookmarks"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Add and View Bookmarks (Priority: P1)

As a user, I want to quickly add bookmarks from the command line and view them later, so I can capture interesting URLs without interrupting my workflow.

**Why this priority**: This is the core value proposition - capturing and retrieving bookmarks. Without this, the tool has no purpose.

**Independent Test**: Can be fully tested by adding a bookmark with `bookmark add <url>` and retrieving it with `bookmark list`, delivering immediate value as a basic bookmark storage system.

**Acceptance Scenarios**:

1. **Given** I have a URL I want to save, **When** I run `bookmark add https://example.com`, **Then** the bookmark is saved and confirmed
2. **Given** I have saved multiple bookmarks, **When** I run `bookmark list`, **Then** all my bookmarks are displayed in a readable format
3. **Given** I want to add context, **When** I run `bookmark add https://example.com --title "Example Site" --note "Useful resource"`, **Then** the bookmark is saved with title and note
4. **Given** I have no bookmarks saved, **When** I run `bookmark list`, **Then** I see a message indicating no bookmarks exist

---

### User Story 2 - Tag-Based Organization (Priority: P2)

As a user, I want to organize bookmarks with tags, so I can categorize and find related bookmarks easily.

**Why this priority**: Tags enable organization beyond simple lists, making the tool scalable for large bookmark collections. This is the primary organizational mechanism per the constitution.

**Independent Test**: Can be tested by adding tags to bookmarks with `bookmark add <url> --tag python --tag tutorial` and searching by tag with `bookmark search --tag python`, delivering a fully functional tag-based organization system.

**Acceptance Scenarios**:

1. **Given** I'm adding a bookmark, **When** I include `--tag programming --tag python`, **Then** the bookmark is tagged with both tags
2. **Given** I have bookmarks with various tags, **When** I run `bookmark search --tag python`, **Then** only bookmarks tagged with "python" are shown
3. **Given** I have bookmarks, **When** I run `bookmark tags list`, **Then** I see all unique tags with bookmark counts
4. **Given** I want to refine search, **When** I run `bookmark search --tag python --tag tutorial`, **Then** only bookmarks with both tags are shown

---

### User Story 3 - Import Existing Bookmarks (Priority: P3)

As a user, I want to import bookmarks from my browser or other tools, so I can consolidate my existing bookmarks into this system.

**Why this priority**: Reduces friction for adoption by allowing users to migrate existing bookmarks rather than starting from scratch.

**Independent Test**: Can be tested by exporting bookmarks from a browser to HTML format, then running `bookmark import bookmarks.html`, validating that all bookmarks are correctly imported with titles and folders converted to tags.

**Acceptance Scenarios**:

1. **Given** I have a browser bookmarks HTML export file, **When** I run `bookmark import bookmarks.html`, **Then** all bookmarks are imported with titles
2. **Given** my browser export has folder hierarchy, **When** I import it, **Then** folder names are converted to tags
3. **Given** the import file contains duplicates, **When** I import it, **Then** duplicates are detected and I'm prompted to skip or merge them
4. **Given** I want to review before import, **When** I run `bookmark import bookmarks.html --dry-run`, **Then** I see what would be imported without actually importing

---

### User Story 4 - Search and Filter (Priority: P2)

As a user, I want to search bookmarks by URL, title, tags, or notes, so I can quickly find specific bookmarks in a large collection.

**Why this priority**: Search is essential for usability once a user has more than a handful of bookmarks. Combined with tags, it provides powerful discovery.

**Independent Test**: Can be tested by adding several bookmarks with different attributes, then using `bookmark search <query>` to find specific ones by text matching or tag filtering.

**Acceptance Scenarios**:

1. **Given** I remember part of a URL, **When** I run `bookmark search example.com`, **Then** all bookmarks containing "example.com" are shown
2. **Given** I remember a keyword from title or notes, **When** I run `bookmark search "python tutorial"`, **Then** bookmarks matching that text are shown
3. **Given** I want combined filters, **When** I run `bookmark search --tag python "web scraping"`, **Then** only python-tagged bookmarks matching "web scraping" are shown
4. **Given** I want case-insensitive search, **When** I search for any case variation, **Then** results are returned regardless of case

---

### User Story 5 - Export and Backup (Priority: P3)

As a user, I want to export my bookmarks to standard formats, so I can use them with other tools or create backups.

**Why this priority**: Data portability is a constitutional principle. This ensures users maintain ownership and control of their data.

**Independent Test**: Can be tested by running `bookmark export --format json output.json` and verifying the output file contains all bookmarks in valid JSON format that can be imported elsewhere.

**Acceptance Scenarios**:

1. **Given** I want a backup, **When** I run `bookmark export --format json backup.json`, **Then** all bookmarks are exported to JSON format
2. **Given** I want browser compatibility, **When** I run `bookmark export --format html bookmarks.html`, **Then** bookmarks are exported in Netscape bookmark HTML format
3. **Given** I want to share specific bookmarks, **When** I run `bookmark export --tag python --format json python-links.json`, **Then** only tagged bookmarks are exported
4. **Given** I want machine-readable output, **When** I run `bookmark list --json`, **Then** bookmarks are output as JSON to stdout

---

### Edge Cases

- What happens when adding a duplicate URL? System should detect and offer to update or skip.
- How does system handle malformed URLs? System should validate and provide helpful error messages.
- What happens when importing a very large bookmark file (10,000+ bookmarks)? System should show progress and handle efficiently.
- How does system handle tags with spaces or special characters? System should allow quoted tags or use consistent tag format.
- What happens when the bookmarks data file is corrupted? System should detect corruption and offer recovery from backup if available.
- How does system handle concurrent access (two processes modifying bookmarks simultaneously)? System should use file locking or detect conflicts.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow users to add bookmarks with URL, optional title, optional notes, and optional tags via command line
- **FR-002**: System MUST list all bookmarks in human-readable format showing URL, title, tags, and creation date
- **FR-003**: System MUST support searching bookmarks by URL substring, title text, note text, or tag combinations
- **FR-004**: System MUST support multiple tags per bookmark
- **FR-005**: System MUST import bookmarks from HTML format (Netscape bookmark file format used by major browsers)
- **FR-006**: System MUST export bookmarks to JSON and HTML formats
- **FR-007**: System MUST detect duplicate URLs when adding bookmarks and provide options to update or skip
- **FR-008**: System MUST validate URLs before saving (basic URL format validation)
- **FR-009**: System MUST display all unique tags with usage counts via a tags list command
- **FR-010**: System MUST support filtering search results by one or more tags
- **FR-011**: System MUST provide JSON output format for all list and search commands via --json flag
- **FR-012**: System MUST store bookmarks in plain text JSON format in a user-accessible location
- **FR-013**: System MUST allow users to edit bookmark details (title, notes, tags) after creation
- **FR-014**: System MUST allow users to delete bookmarks by URL or by selecting from list
- **FR-015**: System MUST provide helpful error messages for invalid commands, missing arguments, or file operation failures

### Key Entities

- **Bookmark**: Represents a saved URL with metadata
  - url: The web address (required, unique)
  - title: Display name for the bookmark (optional, defaults to URL)
  - notes: User-added context or description (optional)
  - tags: List of tag strings for categorization (optional, multiple allowed)
  - created: Timestamp when bookmark was added
  - modified: Timestamp when bookmark was last updated

- **Tag**: A label used to categorize bookmarks
  - name: The tag string (case-insensitive)
  - Associated with multiple bookmarks (many-to-many relationship)

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can add a new bookmark in less than 10 seconds via single command
- **SC-002**: Users can find a specific bookmark by tag or keyword in less than 5 seconds
- **SC-003**: Users can successfully import a standard browser bookmark export file with 100% of valid bookmarks captured
- **SC-004**: System handles bookmark collections of 10,000+ bookmarks with search results returned in under 2 seconds
- **SC-005**: 95% of users can understand and use core commands (add, list, search) without reading documentation
- **SC-006**: Users can export their complete bookmark collection to JSON or HTML format with 100% data fidelity
- **SC-007**: All bookmark data remains in human-readable, version-control-friendly plain text format

## Assumptions

- Users have Python 3.9+ installed on their system
- Users are comfortable using command-line interfaces
- Bookmarks will be stored in user's home directory (e.g., `~/.bookmarks/bookmarks.json`)
- Initial version targets single-user local usage (no cloud sync or multi-user support)
- Users will manage their own backups or use version control for bookmark files
- Browser import format is standard Netscape bookmark HTML format used by Chrome, Firefox, Safari, and Edge
- Search is case-insensitive and supports partial text matching
- Tag matching is exact (case-insensitive) but search term matching is substring-based
- No GUI or web interface in initial version (pure CLI)
- System will use file-based storage (no database) for simplicity and portability
