
class Account {
    constructor() {
        console.log("Initialized Account");
        this.loginForm = document.getElementById("account");
        this.events();
    }

    events() {
        console.log(document.getElementById("demoLoginLink").innerHTML);
        document.getElementById("demoLoginLink").addEventListener("click", () => this.displayLoginForm(true));
        document.getElementById("backToLoginLink").addEventListener("click", () => this.displayLoginForm(false));
    }   

    async displayLoginForm(isDemo) {
        const res = await fetch(`Account/DisplayLoginForm?isDemo=${isDemo}`);
        const formHTML = await res.text();
        console.log(formHTML);
        this.loginForm.innerHTML = formHTML;  
    }
}

const account = new Account();