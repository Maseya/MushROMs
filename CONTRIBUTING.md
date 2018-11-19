# Contributing

## Welcome!

Thanks for taking an interest in my work. Before getting started, read
over this document to see how contributions should be submitted. These
guidelines will help all of us best understand each other and
collaborate together.

## Table of Contents

* [How to file a bug report](#how-to-file-a-bug-report)
* [How to suggest a new feature](#how-to-suggest-a-new-feature)
* [How to set up your environment and run tests](#how-to-set-up-your-environment-and-run-tests)
* [The types of contributions we're looking for](#the-types-of-contributions-were-looking-for)
* [Our vision for the project](#our-vision-for-the-project)
* [Code styles](#code-styles)
    - [Commit messages](#commit-messages)

## How to file a bug report

To file a bug report, visit the [Issues][issues] page, and search if the
bug has already been reported. If it has not, then open a new issue,
giving a short, descriptive title explaining the bug. From there, you
will get a template file that outlines how to describe your bug and what
information we are looking for when you submit it.

## How to suggest a new feature

Same as filing a bug report, open a new [Issue][issues] and follow the
guidelines in the template.

## How to set up your environment and run tests

Refer to the [README](README.md#installation) on the front page for
complete setup instructions.

## The types of contributions we're looking for

Refer to our [DESCRIPTION](DESCRIPTION.md) file to better understand
what this project is about. To summarize the points it makes,
the following contributions are greatly supported:

* Unit/Benchmark tests
* Source code documentation
* Bug fixes
* Cross-platform support
* Cross-IDE support
* Issue closers
* Features that are related to this project's vision.

Below are some things we think wouldn't be helpful for contributions

* Creating a feature request with no documentation or understanding of
the feature.
* Submitting a pull request with no description of what the code does.
Even if it's a typo fix, mention that it's a typo fix.
* Submitting a large pull request without opening an Issue first. If you
have a large feature planned, create an Issue so other contributors can
discuss it with you. Open the pull request early, or link to your
branch, even when it is unfinished, so code can be open for review.
* Submitting a bug fix without an accompanying unit test to catch the
bug from now on (except in cases of bugs that were typos e.g.
`if (x = true)` to `if (x == true)`.
* Requesting a feature that is far outside the scope of the project's
vision.
* Submitting a bug report without steps to reproduce.

## Our vision for the project

Refer to the [DESCRIPTION](DESCRIPTION.md) for a complete picture of our
project's vision.

## Code styles

Refer to our [CODE STYLES](CODE_STYLES.md) guide for an in-depth
discussion on how to write your code for this project.

### Commit messages

[This][commit] article does a great job describing the value of a
formatted git commit message. The things to take away from it are

1. [Separate subject from body with a blank line][separate]
2. [Limit the subject line to 50 characters][limit]
3. [Capitalize the subject line][capitalize]
4. [Do not end the subject line with a period][period]
5. [Use the imperative mood in the subject line][imperative]
6. [Wrap the body at 72 characters][wrap]
7. [Use the body to explain _what_ and _why_ vs. _how_][explain]

Try to comply to these rules as much as possible.

[issues]: https://github.com/Maseya/Editors/issues
[commit]: https://chris.beams.io/posts/git-commit
[separate]: https://chris.beams.io/posts/git-commit/#separate
[limit]: https://chris.beams.io/posts/git-commit/#limit-50
[capitalize]: https://chris.beams.io/posts/git-commit/#capitalize
[period]: https://chris.beams.io/posts/git-commit/#end
[imperative]: https://chris.beams.io/posts/git-commit/#imperative
[wrap]: https://chris.beams.io/posts/git-commit/#wrap-72
[epxlain]: https://chris.beams.io/posts/git-commit/#why-not-how
