<!--
Sync Impact Report:
- Version change: 0.0.0 → 1.0.0
- Initial constitution creation
- All principles defined for bookmark organizer CLI tool
- Templates: All remain aligned with new constitution
-->

# Bookmark Organizer Constitution

## Core Principles

### I. CLI-First Design
Every feature must be accessible through command-line interface with clear, intuitive commands. The tool MUST:
- Accept input via stdin, command arguments, or file paths
- Output results to stdout in both human-readable and machine-parseable formats
- Report errors to stderr with actionable messages
- Support piping and integration with other command-line tools

**Rationale**: CLI-first ensures scriptability, automation, and integration into existing workflows.

### II. Data Portability
All bookmark data MUST be stored in open, human-readable formats (JSON, Markdown, or similar). The tool MUST:
- Allow import from common bookmark formats (HTML, JSON, browser exports)
- Enable export to multiple formats for compatibility
- Never lock users into proprietary formats
- Maintain data integrity during import/export operations

**Rationale**: Users must retain full ownership and control of their bookmark data.

### III. Simple Storage Model
Bookmark storage MUST remain simple and version-control friendly:
- Plain text files that can be tracked with git
- No database required for basic functionality
- Clear file structure that users can navigate and edit manually
- Support for both single-file and directory-based organization

**Rationale**: Simplicity ensures maintainability and gives users direct access to their data.

### IV. Tag-Based Organization
Tags are the primary organizational mechanism. The tool MUST:
- Support multiple tags per bookmark
- Enable hierarchical tag structures (parent/child relationships)
- Provide tag search and filtering capabilities
- Auto-suggest tags based on URL patterns and existing tags

**Rationale**: Tags provide flexible, multi-dimensional organization superior to folder hierarchies.

### V. Testing and Quality
All features MUST be tested before deployment:
- Unit tests for core functionality
- Integration tests for file operations and data transformations
- Example-based testing with real bookmark collections
- Error handling verification

**Rationale**: CLI tools must be reliable; data corruption or loss is unacceptable.

## Technology Constraints

The tool MUST be implemented in Python 3.9+ for the following reasons:
- Wide availability across platforms (Windows, macOS, Linux)
- Rich ecosystem for CLI development (click, typer, argparse)
- Strong text processing and JSON handling capabilities
- Easy distribution via pip or standalone executables

External dependencies should be minimized. Prefer standard library when possible.

## User Experience Standards

### Command Structure
- Commands follow verb-noun pattern: `bookmark add`, `tag list`, `search filter`
- Consistent flag naming: `--tag`, `--output`, `--format`
- Provide helpful error messages with examples
- Include `--help` for every command and subcommand

### Output Formats
- Default: Human-readable formatted output with colors
- `--json`: Machine-parseable JSON output
- `--quiet`: Minimal output for scripting
- `--verbose`: Detailed operation logs

## Governance

This constitution supersedes all other development practices and guides all technical decisions. Changes to this constitution require:
1. Documented rationale for the proposed amendment
2. Impact assessment on existing features
3. Version bump according to semantic versioning

**Version Control**:
- MAJOR: Breaking changes to constitution principles
- MINOR: Addition of new principles or sections
- PATCH: Clarifications or non-semantic refinements

All feature proposals, code reviews, and architectural decisions MUST verify compliance with these principles.

**Version**: 1.0.0 | **Ratified**: 2026-04-28 | **Last Amended**: 2026-04-28
