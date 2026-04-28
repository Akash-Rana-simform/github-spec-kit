# Research: Bookmark Organizer CLI

**Phase**: 0 (Outline & Research)  
**Created**: 2026-04-28  
**Purpose**: Resolve technical unknowns and document technology decisions

## Technology Decisions

### 1. CLI Framework: Click

**Decision**: Use Click library for CLI interface

**Rationale**:
- Well-established, mature Python CLI framework with excellent documentation
- Provides automatic help generation, argument parsing, and validation
- Supports nested command groups (e.g., `bookmark add`, `bookmark tags list`)
- Better developer experience than argparse with decorators and less boilerplate
- Used by major projects (Flask, Black, pipenv)
- Lighter weight than alternatives like Typer while providing sufficient functionality

**Alternatives Considered**:
- **argparse** (stdlib): More verbose, less intuitive API; no nested commands without extra complexity
- **Typer**: Adds type hints and modern Python features, but adds dependency; Click is more than sufficient for our needs
- **docopt**: Elegant but less flexible for complex command structures

**References**:
- Click documentation: https://click.palletsprojects.com/
- Click vs argparse comparison: Click provides cleaner syntax and better UX

---

### 2. Data Storage Format: JSON

**Decision**: Store bookmarks in JSON format in `~/.bookmarks/bookmarks.json`

**Rationale**:
- Human-readable and manually editable (constitution requirement)
- Git-friendly - clear diffs when bookmarks change
- Native Python support via stdlib json module (no dependencies)
- Easy to validate and recover from corruption
- Widely compatible - can be processed by any programming language or tool
- Efficient enough for 50k+ bookmarks (modern JSON parsers are fast)

**Alternatives Considered**:
- **SQLite**: More scalable but violates "no database" constitution requirement; not human-readable
- **YAML**: Slightly more readable but slower to parse, requires dependency, no significant benefit over JSON
- **Plain text/Markdown**: Difficult to maintain structured data and relationships (tags, metadata)
- **CSV**: Poor fit for nested/relational data (multiple tags per bookmark)

**Storage Schema**:
```json
{
  "bookmarks": [
    {
      "url": "https://example.com",
      "title": "Example Site",
      "notes": "Useful resource",
      "tags": ["python", "tutorial"],
      "created": "2026-04-28T18:30:00Z",
      "modified": "2026-04-28T18:30:00Z"
    }
  ],
  "metadata": {
    "version": "1.0",
    "last_modified": "2026-04-28T18:30:00Z"
  }
}
```

---

### 3. Browser Import Format: Netscape Bookmark HTML

**Decision**: Support Netscape Bookmark File Format for HTML imports

**Rationale**:
- De facto standard used by all major browsers (Chrome, Firefox, Safari, Edge)
- Well-documented format with predictable structure
- Python's HTMLParser from stdlib can handle parsing
- Folder hierarchy in HTML export maps naturally to our tag system

**Parsing Strategy**:
- Use `html.parser.HTMLParser` from stdlib to parse bookmark HTML
- Convert `<DL>` nested lists to folder/subfolder structure
- Convert folders to tags (e.g., folder "Programming/Python" → tags ["programming", "python"])
- Extract URL from `<A HREF>`, title from link text, timestamps from ADD_DATE attribute

**Alternatives Considered**:
- **Browser-specific APIs**: Would require browser-specific code and user setup
- **BeautifulSoup**: Adds dependency when stdlib HTMLParser is sufficient for well-formed bookmark HTML

---

### 4. Search Implementation: In-Memory Filtering

**Decision**: Load all bookmarks into memory and filter with Python

**Rationale**:
- Simple implementation with no external dependencies
- Fast enough for 50k+ bookmarks (modern computers have plenty of RAM)
- Enables complex search patterns (text matching + tag filtering + boolean logic)
- Allows case-insensitive search with `.lower()` comparisons
- No indexing complexity - just iterate and filter

**Search Performance**:
- 50k bookmarks × ~500 bytes/bookmark = ~25MB in memory (acceptable)
- Linear scan of 50k items with Python string operations: ~10-50ms on modern hardware
- Well under 2-second requirement even with complex queries

**Optimizations** (if needed later):
- Cache loaded bookmarks between commands (not needed for v1 - load time is negligible)
- Add indexes for tags (premature optimization - not needed until >100k bookmarks)

**Alternatives Considered**:
- **Full-text search engine** (Elasticsearch, Whoosh): Massive overkill for this scale; violates simplicity principle
- **SQLite FTS**: Requires database; not human-readable

---

### 5. Testing Strategy

**Decision**: pytest with three test layers

**Test Layers**:

1. **Unit Tests** (`tests/unit/`)
   - Test individual modules in isolation
   - Mock file I/O to test storage logic without actual files
   - Test search algorithms with sample data
   - Test import/export parsing logic with fixture files

2. **Integration Tests** (`tests/integration/`)
   - Test CLI commands end-to-end using Click's testing utilities
   - Create temporary bookmark files for each test
   - Verify actual file operations work correctly
   - Test import from real browser export files

3. **Fixtures** (`tests/fixtures/`)
   - `sample_bookmarks.json`: Various bookmark scenarios
   - `sample_export.html`: Real browser bookmark export
   - `large_collection.json`: 10k+ bookmarks for performance testing

**pytest Plugins**:
- `pytest-cov`: Code coverage reporting
- `pytest-mock`: Mocking utilities (only if needed; prefer stdlib unittest.mock)
- Click's built-in `CliRunner`: Test CLI commands without subprocess overhead

**Rationale**:
- pytest is Python standard for modern testing
- Clear separation of concerns (unit vs integration)
- Fast test execution with minimal setup
- Easy to run locally and in CI/CD

---

### 6. Error Handling Strategy

**Decision**: Graceful failure with helpful error messages

**Error Categories**:

1. **User Input Errors**: Invalid URLs, missing required arguments
   - Show clear error message with example of correct usage
   - Exit with code 1

2. **File Operation Errors**: Corrupted JSON, permission denied, disk full
   - Detect corruption and offer to restore from backup (if exists)
   - Show filesystem error with suggested fix
   - Exit with code 2

3. **Import Errors**: Malformed HTML, unsupported format
   - Show which bookmarks failed to import and why
   - Continue importing valid bookmarks (partial success)
   - Exit with code 0 but show warning count

**Best Practices**:
- Never lose user data - atomic file writes (write to temp, then rename)
- Auto-backup before destructive operations (optional `--no-backup` flag)
- Validate JSON schema on load; offer to fix common issues
- Use Python's pathlib for cross-platform path handling

---

## Best Practices Research

### Python CLI Best Practices

1. **Entry Point**: Use `console_scripts` in `setup.py` or `pyproject.toml` for clean installation
2. **Configuration**: Support `~/.bookmarks/config.json` for user preferences (future enhancement)
3. **Output Formatting**: Use rich/colorama for colored output (optional future enhancement)
4. **Logging**: Use stdlib logging for `--verbose` mode debugging
5. **Cross-platform**: Use pathlib, avoid hardcoded paths, test on Windows/Mac/Linux

### JSON Best Practices

1. **Atomic Writes**: Write to temporary file, then `os.replace()` to ensure atomicity
2. **Indentation**: Pretty-print JSON with `indent=2` for human readability
3. **Encoding**: Always use UTF-8 explicitly
4. **Schema Validation**: Check for required fields on load, provide clear error if missing
5. **Backward Compatibility**: Include version field in metadata to support future schema migrations

### Tag Management Best Practices

1. **Normalization**: Convert all tags to lowercase for consistency
2. **Whitespace**: Allow spaces in tags but trim leading/trailing whitespace
3. **Special Characters**: Allow alphanumeric, spaces, hyphens, underscores; reject others
4. **Hierarchical Tags**: Support `/` separator for hierarchy (e.g., `programming/python`)
5. **Tag Suggestions**: Auto-suggest tags based on URL patterns (e.g., github.com → suggest "github", "code")

---

## Open Questions (None)

All technical unknowns have been resolved. No clarifications needed before proceeding to Phase 1 design.
