// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

import Roles from "./roles.js";

class BugTracker {
    constructor() {
        console.log("Initialized JavaScript Startup");
        this.roles = new Roles();
    }
}

window.onload = () => new BugTracker();