
class Project {
    constructor() {
        if (document.getElementById("projectInfoContainer")) {
            console.log("Project Initialized");
            this.deleteProject = this.deleteProject.bind(this);
            this.events();
        }               
    }

    events() {
        document.getElementById("deleteProjectBtn").addEventListener("click", this.deleteProject);
    }


}