﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

import RoleList from "./role/roleList.js";
import ProjectList from "./project/projectList.js";
import Project from "./project/project.js";

class BugTracker {
    constructor() {
        console.log("Initialized JavaScript Startup");
        this.roleList = new RoleList();
        this.projectList = new ProjectList();
        this.project = new Project();
    }
}

window.onload = () => new BugTracker();