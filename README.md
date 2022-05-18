# Bugtrace

A bug/issue tracking web application built with C# .NET MVC.

![alt text](https://github.com/Terioch/bug-tracker/blob/main/BugTracker/wwwroot/images/bugtrace.png?raw=true)

## General Information

This application enables the storing and maintenance of issues in the form of tickets for a collection of software projects. 
Each ticket is represented by the following properties:

* Title
* Description
* Submitted Date
* Type
* Status
* Priority
* Associated Project
* Assigned Developer
* Ticket Submitter
* Records of prior ticket modifications
* Image Attachments
* Ticket Comments

Furthermore, authenticated users can be assigned one of the following roles which restricts their access level:

* Owner (Full unrestricted access)
* Admin (Full access but cannot control role management)
* Project Manager (Can manage tickets for projects they're assigned to)
* Developer (Can add comments, attachments and update the status of a ticket they're assigned to)
* Submitter (Can submit tickets for projects they're assigned to)

The project has been configured for demonstration purposes and can be freely accessed with demo accounts assigned to the above roles excluding the Owner role.

## Technologies

* C#
* .NET
* MVC (architecture)
* Entity Framework (data-access)
* Identity Framework (authentication + authorization)
* Bootstrap (UI)

## Launch 

This application is hosted with Heroku and can be found [here](https://bugtrace.herokuapp.com/).

## Contact Information

[Email](riostockton@gmail.com) | [Portfolio](https://terioch.github.io/portfolio-site/)