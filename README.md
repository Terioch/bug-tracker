# Bug Tracker

A bug/issue tracking web application built using Asp.Net MVC.

## General Information

This project stores and maintains issues in the form of tickets for a collection of projects. Each ticket contains the following information:

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

Authentication and role-based authorization were implemented using Identity framework, whilst data-access was configured using Entity. 
Each authenticated user can be assigned one of the following roles which applies a restriction to their access level:

* Owner (Full unrestricted access)
* Admin (Full access but cannot control role management)
* Project Manager (Can manage tickets for projects they're assigned to)
* Developer (Can add comments, attachments and update the status of a ticket they're assigned to)
* Submitter (Can submit tickets for projects they're assigned to)

The project has been configured for demonstration purposes and can be freely accessed with demo accounts assigned to the above roles excluding Owner.

## Launch 

This application is hosted on Heroku at https://dotnet-bug-tracker.herokuapp.com/