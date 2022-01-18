
class Ticket {
    constructor() {
        if (document.getElementById("ticketContainer")) {
            console.log("Initialized Ticket");
            this.ticketContainer = document.getElementById("ticketContainer");
            // this.commentListClickHandler = this.commentListClickHandler.bind(this);
            this.handleCommentCreation = this.handleCommentCreation.bind(this);           
            // this.handleCommentDeletion = this.handleCommentDeletion.bind(this);
            this.filterCommentList = this.filterCommentList.bind(this);
            this.filterHistoryList = this.filterHistoryList.bind(this);
            this.events();
        }
    }

    events() {
        document.getElementById("createCommentBtn").addEventListener("click", this.handleCommentCreation);
        // document.getElementById("commentListGroup").addEventListener("click", this.commentListClickHandler);       
        document.getElementById("commentListSearchInput").addEventListener("keyup", this.filterCommentList);
        document.getElementById("historyListSearchInput").addEventListener("keyup", this.filterHistoryList);
    }

    findNearestParentElement(el, className) {
        while (!el.classList.contains(className)) {
            el = el.parentElement;
        }
        return el;
    }   

    /*commentListClickHandler(e) {
        const commentId = this.findNearestParentElement(e.target, "comment-list-item").getAttribute("data-commentId");
        console.log(commentId);
        
        if (e.target.classList.contains("delete-comment-btn")) {
            this.handleCommentDeletion(commentId);
        }
    }*/

    async handleCommentCreation() {
        const id = this.ticketContainer.getAttribute("data-id");
        const commentsContainer = document.getElementById("commentListContainer"); 
        const commentDescriptionInput = document.getElementById("commentDescriptionInput");

        try {
            const res = await fetch(`/ticketComment/create/${id}`, {
                method: "POST",
                headers: {
                    "Content-Type": "Application/Json",
                },
                body: JSON.stringify({
                    value: commentDescriptionInput.value,
                })
            });
            
            const commentListHTML = await res.text();            
            if (res.status == 400) throw commentListHTML; // Throw initial error                
            commentsContainer.innerHTML = commentListHTML;
            document.getElementById("commentListGroup").addEventListener("click", this.commentListClickHandler); // Attach event listener to replacement list
            commentDescriptionInput.value = "";
        } catch (err) {
            console.error(err);
            document.getElementById("commentCreationValidationErrors").innerHTML = err;
        }
    }        

    async filterCommentList(e) {
        console.log(e.target.value);
        const ticketId = this.ticketContainer.getAttribute("data-id");
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/ticketComment/filterCommentsByAuthorReturnPartial?id=${ticketId}&searchTerm=${searchTerm}`);
            const commentListHTML = await res.text();
            document.getElementById("commentListContainer").innerHTML = commentListHTML;
        } catch (err) {
            console.error(err);
        }
    }    

    async filterHistoryList(e) {
        const ticketId = this.ticketContainer.getAttribute("data-id");
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/ticketHistory/filterHistoryReturnPartial?id=${ticketId}&searchTerm=${searchTerm}`);
            const historyListHTML = await res.text();
            document.getElementById("historyListContainer").innerHTML = historyListHTML;
        } catch (err) {
            console.error(err);
        }
    }

    /*async handleCommentDeletion(commentId) {
        const commentsContainer = document.getElementById("commentListContainer");                 

        try {
            const res = await fetch(`/ticketComment/delete/${commentId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "Application/Json",
                },
            });

            const commentListHTML = await res.text();
            commentsContainer.innerHTML = commentListHTML;           
            document.getElementById("commentListGroup").addEventListener("click", this.commentListClickHandler); // Attach event listener to replacement list                 
        } catch (err) {
            console.error(err);
        }
    }*/
}

export default Ticket;