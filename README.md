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

A the end of each sprint, we will merge the `develop` branch to `main` and create a `sprint-X` (where X is the number of the sprint) branch off `main` to capture the snapshot of the repo at this time. No further commits should be made to `sprint` branches.

## Sprints

### Sprint 1

![Our code structure after Sprint 1](sprint1.png)

In sprint 1 we created an MVP Console app in which you can:

- create a user that's either a tutor or a student,
- log in using UserName and UserId,
- create ads for lessons and add available lesson dates for each ad as a tutor,
- browse all lesson ads and request acceptance for any of them and after being accepted, browse dates of lessons and request acceptance for them as a student,
- accept any number of students for each ad and single student for a lesson date.
