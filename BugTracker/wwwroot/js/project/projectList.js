
class ProjectList {
    constructor() {
        if (document.getElementById("projectSearchInput")) {
            console.log("Initialized projects");
            this.filterProjects = this.filterProjects.bind(this);
            this.events();
        }        
    }

    events() {
        document.getElementById("projectSearchInput").addEventListener("keyup", this.filterProjects);
    }    

    async filterProjects(e) {       
        const projectListContainer = document.getElementById("projectListContainer");        
        const searchTerm = e.target.value.toLowerCase();        
        
        try {
            const res = await fetch(`/project/filterProjectsByNameReturnPartial/${searchTerm}`);
            const projectListHTML = await res.text();        
            projectListContainer.innerHTML = projectListHTML;
        } catch (err) {
            console.error(err);            
        }
    }
}

export default ProjectList;