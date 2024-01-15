# jcszr12-Jaszczur

## Project description

This is an app that lets tutors or private teachers connect with their clients and schedule lessons.

## Our Team

- Grzegorz Kmieciński
- Adam Monikowski
- Paulina Wudarska

## Branches

The 2 main long-lived branches are:

- `main`
- `develop`

When we start working on a new task, we create a new `feature` branch based on `develop`. This branch should be called `feature/ja-xx-task-name`, where `ja-xx` is the Jira number of the task. The `task-name` part doesn't have to exactly correspond to the task's name in Jira.

When the person working on the tasks considers it done, they should create a pull request back to the `develop` branch. If we collectively agree that it's ready, we accept the pull request and mark the task as done in Jira.

A the end of each sprint, we will merge the `develop` branch to `main` and create a `sprintX` (where X is the number of the sprint) branch off `main` to capture the snapshot of the repo at this time. No further commits should be made to `sprint` branches.
