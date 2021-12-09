
class ProjectList {
    constructor() {
        if (document.getElementById("projectListPageContainer")) {
            console.log("Initialized Project List");
            this.filterProjectList = this.filterProjectList.bind(this);
            this.events();
        }        
    }

    events() {
        document.getElementById("projectListSearchInput").addEventListener("keyup", this.filterProjectList);
    }    

    async filterProjectList(e) {       
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