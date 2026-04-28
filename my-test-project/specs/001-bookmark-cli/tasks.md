# Tasks: Bookmark Organizer CLI

**Input**: Design documents from `specs/001-bookmark-cli/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/cli-commands.md

**Tests**: Tests are included in this implementation plan as specified in the constitution (Principle V: Testing and Quality).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

All paths are relative to repository root. Project uses single project structure:
- Source code: `src/bookmark_cli/`
- Tests: `tests/unit/`, `tests/integration/`, `tests/fixtures/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [ ] T001 Create project directory structure per plan.md (src/bookmark_cli/, tests/, docs/)
- [ ] T002 Initialize Python package with pyproject.toml and setup.py following Python best practices
- [ ] T003 [P] Configure pytest with coverage reporting in pyproject.toml
- [ ] T004 [P] Add Click dependency (CLI framework) to pyproject.toml
- [ ] T005 [P] Create src/bookmark_cli/__init__.py with version info
- [ ] T006 Create .gitignore for Python project (venv, __pycache__, *.pyc, .pytest_cache)
- [ ] T007 Create README.md with project overview and installation instructions

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [ ] T008 Create Bookmark data model in src/bookmark_cli/models.py (url, title, notes, tags, created, modified)
- [ ] T009 Create Storage module in src/bookmark_cli/storage.py with load_bookmarks() and save_bookmarks() functions
- [ ] T010 [P] Implement atomic file write operation in storage.py (temp file + os.replace)
- [ ] T011 [P] Create bookmark file location handling (~/.bookmarks/ directory, bookmarks.json file)
- [ ] T012 [P] Add URL validation function in src/bookmark_cli/models.py
- [ ] T013 [P] Add tag validation and normalization function in src/bookmark_cli/models.py (lowercase, trim whitespace)
- [ ] T014 Create CLI entry point in src/bookmark_cli/cli.py with Click root command group
- [ ] T015 [P] Add global CLI options (--help, --version, --verbose, --quiet, --json) to root command
- [ ] T016 [P] Setup tests/fixtures/ directory with sample_bookmarks.json for testing

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Add and View Bookmarks (Priority: P1) 🎯 MVP

**Goal**: Enable users to add bookmarks and view them in a list

**Independent Test**: Add a bookmark with `bookmark add <url>`, retrieve it with `bookmark list`, verify it appears in output

### Tests for User Story 1

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T017 [P] [US1] Unit test for Bookmark model creation in tests/unit/test_models.py
- [ ] T018 [P] [US1] Unit test for storage.load_bookmarks() with empty file in tests/unit/test_storage.py
- [ ] T019 [P] [US1] Unit test for storage.save_bookmarks() with atomic write in tests/unit/test_storage.py
- [ ] T020 [P] [US1] Integration test for `bookmark add` command in tests/integration/test_cli_commands.py
- [ ] T021 [P] [US1] Integration test for `bookmark list` command in tests/integration/test_cli_commands.py

### Implementation for User Story 1

- [ ] T022 [US1] Implement `bookmark add` command in src/bookmark_cli/cli.py with options (--title, --note, --tag, --json)
- [ ] T023 [US1] Add duplicate URL detection to `bookmark add` with user prompt (update/skip)
- [ ] T024 [US1] Implement `bookmark list` command in src/bookmark_cli/cli.py with options (--tag, --sort, --reverse, --limit, --json)
- [ ] T025 [US1] Add human-readable formatter for bookmark output (with colors for tags, dates)
- [ ] T026 [US1] Add JSON output formatter for bookmark list (returns structured JSON)
- [ ] T027 [US1] Add error handling for file I/O errors (permission denied, disk full, corrupted JSON)
- [ ] T028 [US1] Add timestamp generation (ISO 8601 format) for created/modified fields

**Checkpoint**: At this point, User Story 1 should be fully functional - users can add and list bookmarks

---

## Phase 4: User Story 2 - Tag-Based Organization (Priority: P2)

**Goal**: Enable tag-based categorization and filtering of bookmarks

**Independent Test**: Add bookmarks with multiple tags, filter by tag with `bookmark search --tag python`, verify only tagged bookmarks appear

### Tests for User Story 2

- [ ] T029 [P] [US2] Unit test for tag normalization (lowercase, trim whitespace) in tests/unit/test_models.py
- [ ] T030 [P] [US2] Unit test for tag validation (reject invalid characters) in tests/unit/test_models.py
- [ ] T031 [P] [US2] Integration test for multiple tags on single bookmark in tests/integration/test_cli_commands.py
- [ ] T032 [P] [US2] Integration test for `bookmark list --tag` filtering in tests/integration/test_cli_commands.py
- [ ] T033 [P] [US2] Integration test for `bookmark tags list` command in tests/integration/test_cli_commands.py

### Implementation for User Story 2

- [ ] T034 [P] [US2] Implement tag filtering logic in src/bookmark_cli/search.py (AND logic for multiple tags)
- [ ] T035 [US2] Update `bookmark list` command to use tag filtering from search.py
- [ ] T036 [US2] Implement `bookmark tags list` command in src/bookmark_cli/cli.py
- [ ] T037 [US2] Add tag counting logic (count bookmarks per tag) in src/bookmark_cli/cli.py
- [ ] T038 [US2] Add sort options for tag list (by name or count, with reverse option)
- [ ] T039 [US2] Add hierarchical tag support (handle `/` separator in tag names)

**Checkpoint**: At this point, User Stories 1 AND 2 work independently - users can organize and filter bookmarks by tags

---

## Phase 5: User Story 4 - Search and Filter (Priority: P2)

**Goal**: Enable text-based search across bookmark URLs, titles, and notes

**Independent Test**: Add bookmarks with various content, search with `bookmark search "python tutorial"`, verify matching bookmarks appear

### Tests for User Story 4

- [ ] T040 [P] [US4] Unit test for case-insensitive text search in tests/unit/test_search.py
- [ ] T041 [P] [US4] Unit test for search across URL, title, and notes in tests/unit/test_search.py
- [ ] T042 [P] [US4] Integration test for `bookmark search <query>` command in tests/integration/test_cli_commands.py
- [ ] T043 [P] [US4] Integration test for combined search + tag filter in tests/integration/test_cli_commands.py

### Implementation for User Story 4

- [ ] T044 [P] [US4] Create Search module in src/bookmark_cli/search.py with search_bookmarks() function
- [ ] T045 [US4] Implement case-insensitive text matching across url, title, notes fields
- [ ] T046 [US4] Implement `bookmark search` command in src/bookmark_cli/cli.py
- [ ] T047 [US4] Add combined filter logic (text search + tag filter) to search.py
- [ ] T048 [US4] Update search command to support --tag option for combined filtering

**Checkpoint**: At this point, User Stories 1, 2, and 4 work independently - full search and filter capabilities available

---

## Phase 6: User Story 3 - Import Existing Bookmarks (Priority: P3)

**Goal**: Enable import from browser bookmark exports (HTML format)

**Independent Test**: Export bookmarks from browser to HTML, run `bookmark import bookmarks.html`, verify bookmarks imported correctly

### Tests for User Story 3

- [ ] T049 [P] [US3] Create fixture file tests/fixtures/sample_export.html with Netscape bookmark format
- [ ] T050 [P] [US3] Unit test for HTML parsing in tests/unit/test_importer.py
- [ ] T051 [P] [US3] Unit test for folder-to-tag conversion in tests/unit/test_importer.py
- [ ] T052 [P] [US3] Integration test for `bookmark import` command in tests/integration/test_cli_commands.py
- [ ] T053 [P] [US3] Integration test for duplicate handling during import in tests/integration/test_cli_commands.py

### Implementation for User Story 3

- [ ] T054 [P] [US3] Create Importer module in src/bookmark_cli/importer.py
- [ ] T055 [US3] Implement HTML parser for Netscape Bookmark File Format using html.parser.HTMLParser
- [ ] T056 [US3] Implement folder hierarchy to tags converter (nested <DL> lists → tag list)
- [ ] T057 [US3] Implement `bookmark import` command in src/bookmark_cli/cli.py
- [ ] T058 [US3] Add format auto-detection (HTML vs JSON) based on file extension and content
- [ ] T059 [US3] Add duplicate handling options (--skip-duplicates, --merge-duplicates, or prompt)
- [ ] T060 [US3] Add --dry-run option to preview import without saving
- [ ] T061 [US3] Add import summary output (added, skipped, merged counts)

**Checkpoint**: At this point, User Stories 1, 2, 3, and 4 work independently - import functionality complete

---

## Phase 7: User Story 5 - Export and Backup (Priority: P3)

**Goal**: Enable export to standard formats (JSON, HTML) for backup and portability

**Independent Test**: Run `bookmark export backup.json`, verify all bookmarks exported in valid JSON format

### Tests for User Story 5

- [ ] T062 [P] [US5] Unit test for JSON export in tests/unit/test_exporter.py
- [ ] T063 [P] [US5] Unit test for HTML export (Netscape format) in tests/unit/test_exporter.py
- [ ] T064 [P] [US5] Integration test for `bookmark export` command in tests/integration/test_cli_commands.py
- [ ] T065 [P] [US5] Integration test for filtered export (--tag option) in tests/integration/test_cli_commands.py

### Implementation for User Story 5

- [ ] T066 [P] [US5] Create Exporter module in src/bookmark_cli/exporter.py
- [ ] T067 [US5] Implement JSON export function in exporter.py (same format as internal storage)
- [ ] T068 [US5] Implement HTML export function in exporter.py (Netscape Bookmark File Format)
- [ ] T069 [US5] Implement `bookmark export` command in src/bookmark_cli/cli.py
- [ ] T070 [US5] Add format auto-detection based on file extension (.html, .json)
- [ ] T071 [US5] Add --format option to explicitly specify export format
- [ ] T072 [US5] Add --tag option to export filtered bookmarks only
- [ ] T073 [US5] Add export success message with file path and count

**Checkpoint**: All user stories complete - full feature set available (add, list, search, tag, import, export)

---

## Phase 8: Additional Commands (Cross-Story Features)

**Purpose**: Commands that operate across multiple user stories

### Tests

- [ ] T074 [P] Unit test for bookmark editing in tests/unit/test_models.py
- [ ] T075 [P] Unit test for bookmark deletion in tests/unit/test_storage.py
- [ ] T076 [P] Integration test for `bookmark edit` command in tests/integration/test_cli_commands.py
- [ ] T077 [P] Integration test for `bookmark delete` command in tests/integration/test_cli_commands.py
- [ ] T078 [P] Integration test for `bookmark tags rename` command in tests/integration/test_cli_commands.py

### Implementation

- [ ] T079 [P] Implement `bookmark edit` command in src/bookmark_cli/cli.py (--title, --note, --add-tag, --remove-tag)
- [ ] T080 [P] Add modified timestamp update on edit operations
- [ ] T081 [P] Implement `bookmark delete` command in src/bookmark_cli/cli.py with confirmation prompt (--yes to skip)
- [ ] T082 [P] Implement `bookmark tags rename` command in src/bookmark_cli/cli.py
- [ ] T083 [P] Add bulk tag rename across all bookmarks
- [ ] T084 Add bookmark not found error messages for edit/delete commands

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Quality, documentation, error handling, and final touches

### Error Handling & Validation

- [ ] T085 [P] Add comprehensive error messages for all CLI commands
- [ ] T086 [P] Add JSON schema validation on file load with recovery suggestions
- [ ] T087 [P] Implement automatic backup before destructive operations (delete, import with merge/replace)
- [ ] T088 [P] Add corruption detection and backup restore command

### Documentation

- [ ] T089 [P] Add detailed docstrings to all public functions and classes
- [ ] T090 [P] Create API documentation from docstrings
- [ ] T091 Create user guide with examples in docs/user-guide.md
- [ ] T092 [P] Add command examples to README.md (based on quickstart.md)
- [ ] T093 [P] Create CONTRIBUTING.md with development setup instructions

### Performance & Quality

- [ ] T094 [P] Create performance test with 10k+ bookmarks in tests/fixtures/large_collection.json
- [ ] T095 [P] Verify search performance meets <2 second requirement for 10k bookmarks
- [ ] T096 [P] Add code coverage check (target: >80% coverage)
- [ ] T097 Run full test suite and fix any failures

### Distribution

- [ ] T098 [P] Configure setuptools entry point for `bookmark` command
- [ ] T099 [P] Test installation with `pip install -e .` in clean virtualenv
- [ ] T100 [P] Create distribution package with `python -m build`
- [ ] T101 Test package installation from wheel file

---

## Dependencies

### User Story Completion Order

```
Phase 1 (Setup) → Phase 2 (Foundation) → [All User Stories Can Start in Parallel]

User Stories (Independent - can be implemented in any order after Foundation):
├── US1: Add and View Bookmarks (P1) 🎯 MVP
├── US2: Tag-Based Organization (P2)
├── US4: Search and Filter (P2)
├── US3: Import Bookmarks (P3)
└── US5: Export and Backup (P3)

Phase 8 (Additional Commands) → depends on US1, US2
Phase 9 (Polish) → depends on all user stories
```

### Suggested MVP Scope

**Minimum Viable Product** (MVP) = Phase 1 + Phase 2 + Phase 3 (User Story 1 only)

This delivers:
- ✅ Add bookmarks with tags, title, notes
- ✅ List all bookmarks
- ✅ Basic duplicate detection
- ✅ JSON storage
- ✅ All required tests for US1

Users can immediately:
- Capture bookmarks from command line
- View their bookmark collection
- Organize with tags (basic support)

**Next Increments**:
- **v0.2**: + US2 (Tag management and filtering)
- **v0.3**: + US4 (Advanced search)
- **v0.4**: + US3 (Import from browser)
- **v0.5**: + US5 (Export for backup)
- **v1.0**: + Phase 8-9 (Edit/delete + polish)

---

## Parallel Execution Opportunities

### Within Foundation Phase (can run in parallel):
- T010 (atomic file write), T011 (file location), T012 (URL validation), T013 (tag validation), T015 (CLI setup), T016 (test fixtures)

### Within US1 (can run in parallel after tests):
- T017, T018, T019, T020, T021 (all tests can run in parallel)
- After tests: T025, T026, T027 (formatters and error handling)

### Within US2 (can run in parallel after tests):
- T029, T030, T031, T032, T033 (all tests)
- After tests: T034, T039 (tag logic)

### Within US4 (can run in parallel):
- T040, T041, T042, T043 (all tests)
- After tests: T044, T045 (search logic)

### Within US3 (can run in parallel):
- T049, T050, T051, T052, T053 (all tests and fixtures)
- After tests: T054, T055 (importer modules)

### Within US5 (can run in parallel):
- T062, T063, T064, T065 (all tests)
- After tests: T066, T067, T068 (exporter modules)

### Within Phase 8:
- T074, T075, T076, T077, T078 (all tests)
- After tests: T079, T080, T081, T082, T083, T084 (all commands are independent)

### Within Phase 9:
- T085, T086, T087, T088 (error handling)
- T089, T090, T091, T092, T093 (all docs)
- T094, T095, T096 (all performance/quality checks)
- T098, T099, T100 (distribution tasks)

---

## Implementation Strategy

### Incremental Delivery Approach

1. **Sprint 1** (MVP - User Story 1): Core add/list functionality
2. **Sprint 2** (US2): Tag organization
3. **Sprint 3** (US4): Search capabilities
4. **Sprint 4** (US3): Import from browser
5. **Sprint 5** (US5): Export for backup
6. **Sprint 6** (Phase 8): Edit/delete commands
7. **Sprint 7** (Phase 9): Polish and release prep

### Development Workflow per User Story

For each user story phase:
1. ✅ Review spec.md for that user story's requirements
2. ✅ Write tests first (TDD approach per constitution)
3. ✅ Run tests → verify they fail (red)
4. ✅ Implement features to make tests pass (green)
5. ✅ Refactor for code quality (refactor)
6. ✅ Verify independent testability of that story
7. ✅ Demo/validate with actual usage before moving to next story

---

## Task Summary

- **Total Tasks**: 101
- **Parallelizable Tasks**: 62 (marked with [P])
- **User Story Tasks**:
  - US1: 12 tasks (T017-T028)
  - US2: 11 tasks (T029-T039)
  - US3: 13 tasks (T049-T061)
  - US4: 9 tasks (T040-T048)
  - US5: 12 tasks (T062-T073)
  - Additional: 9 tasks (T074-T084)
- **Foundation Tasks**: 16 (T001-T016) - must complete before user stories
- **Polish Tasks**: 17 (T085-T101)

**Estimated Timeline**: 6-8 weeks (MVP in 1-2 weeks with US1 only)