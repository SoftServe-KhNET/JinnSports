function AuthView() {
    return {
        self: null,

        show: function () {
            self = this;

            var loginBtn = document.createElement("button");
            loginBtn.id = "login-btn";
            loginBtn.innerHTML = "Login";
            loginBtn.style.display = "inline-block";
            loginBtn.addEventListener("click", this.initLogin);
    
            var registerBtn = document.createElement("button");
            registerBtn.id = "register-btn";
            registerBtn.innerHTML = "Register";
            registerBtn.style.display = "inline-block";
            registerBtn.addEventListener("click", this.initRegister);

            document.getElementById("authentification").appendChild(loginBtn);
            document.getElementById("authentification").appendChild(registerBtn);
        },

        initRegister: function () {

            var background = document.createElement("div");
            background.classList.add("modal-background");

            var loginForm = document.createElement("div");
            loginForm.id = "register-form";
            loginForm.innerHTML = '<p class="caption">Registration</p>';

            var loginInput = document.createElement("input");
            loginInput.type = "text";
            loginInput.id = "login";
            loginInput.classList.add("form-input");
            loginInput.placeholder = "Login";

            var passwordInput = document.createElement("input");
            passwordInput.type = "password";
            passwordInput.id = "password"
            passwordInput.classList.add("form-input");
            passwordInput.placeholder = "Password";

            var passwordRepeatInput = document.createElement("input");
            passwordRepeatInput.type = "password";
            passwordRepeatInput.id = "password-repeat"
            passwordRepeatInput.classList.add("form-input");
            passwordRepeatInput.placeholder = "Password Repeat";

            var nameInput = document.createElement("input");
            nameInput.type = "text";
            nameInput.id = "login-name";
            nameInput.classList.add("form-input");
            nameInput.placeholder = "Name";

            var emailInput = document.createElement("input");
            emailInput.type = "text";
            emailInput.id = "login-email";
            emailInput.classList.add("form-input");
            emailInput.placeholder = "Email";

            var submitButton = document.createElement("input");
            submitButton.type = "button";
            submitButton.id = "click-button";
            submitButton.value = "Submit";
            submitButton.classList.add("form-input");

            var closeBtn = document.createElement("span");
            closeBtn.innerHTML = "X";
            closeBtn.classList.add("close-btn");

            loginForm.appendChild(closeBtn);
            loginForm.appendChild(loginInput);
            loginForm.appendChild(passwordInput);
            loginForm.appendChild(passwordRepeatInput);
            loginForm.appendChild(nameInput);
            loginForm.appendChild(emailInput);
            loginForm.appendChild(submitButton);

            background.appendChild(loginForm);
            document.body.appendChild(background);

            background.classList.add("background-fade-in");

            submitButton.addEventListener("click", self.btnRegClick);

            
            window.onclick = function (event) {
                if (event.target == background) {
                    self.deleteBackground(background);
                }
            }

            closeBtn.onclick = function () {
                self.deleteBackground(background);
            }

            loginInput.onkeydown = function () {
                if (loginInput.classList.contains("invalid")) {
                    loginInput.classList.remove("invalid");
                }
            }
        },

        initLogin: function () {
            var background = document.createElement("div");
            background.classList.add("modal-background");

            var loginForm = document.createElement("div");
            loginForm.id = "login-form";
            loginForm.innerHTML = '<p class="caption">Autentification</p>';

            var loginInput = document.createElement("input");
            loginInput.type = "text";
            loginInput.id = "login";
            loginInput.classList.add("form-input");
            loginInput.placeholder = "Login";

            var passwordInput = document.createElement("input");
            passwordInput.type = "text";
            passwordInput.id = "password"
            passwordInput.classList.add("form-input");
            passwordInput.placeholder = "Password";

            var submitButton = document.createElement("input");
            submitButton.type = "button";
            submitButton.id = "click-button";
            submitButton.value = "Submit";
            submitButton.classList.add("form-input");

            var closeBtn = document.createElement("span");
            closeBtn.innerHTML = "X";
            closeBtn.classList.add("close-btn");

            loginForm.appendChild(closeBtn);
            loginForm.appendChild(loginInput);
            loginForm.appendChild(passwordInput);
            loginForm.appendChild(submitButton);

            background.appendChild(loginForm);
            document.body.appendChild(background);

            background.classList.add("background-fade-in");

            submitButton.addEventListener("click", self.btnLogClick);

            window.onclick = function (event) {
                if (event.target == background) {
                    self.deleteBackground(background);
                }
            }

            closeBtn.onclick = function () {
                self.deleteBackground(background);
            }
        },

        deleteBackground: function (background) {
            background.classList.remove("background-fade-in");
            background.classList.add("background-fade-out");
            setTimeout(function () { background.remove(); }, 1000);
        },

        btnLogClick: function () {

            var credentials = {
                login : document.getElementById("login").value,
                password : document.getElementById("password").value
            };

            var params = JSON.stringify(credentials);

            self.post("/api/Values", params, self.onLoad).then(function (text) {
                self.printMessage(text);
            }, function(error) {
                console.log(error);
            });

            var auth = document.querySelector("p.auth-message");
            if (auth !== null)
                auth.remove(); 
        },

        btnRegClick: function () {

            if (!self.checkLogin() || !self.checkPassword() || !self.checkEmail() || !self.checkName()) {
                return;
            }

            var credentials = {
                login: document.getElementById("login").value,
                password: document.getElementById("password").value,
                name: document.getElementById("login-name").value,
                email: document.getElementById("login-email").value
            };

            var params = JSON.stringify(credentials);

            self.post("/api/Register", params, self.onRegLoad).then(function (text) {
                self.printMessageReg(text);
            }, function (error) {
                console.log(error);
            });

            var auth = document.querySelector("p.auth-message");
            if (auth !== null)
                auth.remove();
        },

        onLoad: function (req) {
            if (req.status == 200) {
                self.showUser(req.responseText);
                return req.responseText;
            }
            else if (req.status == 403)
                return "Wrong password";
            else if (req.status == 404)
                return "User not found";
        },

        onRegLoad: function (req) {
            if (req.status == 200) {
                self.deleteBackground(document.querySelector(".modal-background"));
                return "User created";
            }
            else if (req.status == 403) {
                document.getElementById("login").classList.add("invalid");
                return "Wrong login";
            }
                
        },

        post: function (url, requestuestBody, onLoad) {
            return new Promise(function (succeed, fail) {
                var request = new XMLHttpRequest();
                request.open("POST", url, true);
                request.setRequestHeader('Content-Type', 'application/json');
                request.addEventListener("load", function () {
                    succeed(onLoad(request));
                });
                request.addEventListener("error", function () {
                    fail(new Error("Network error"));
                });
                request.send(requestuestBody);
            });
        },

        printMessageReg: function (message) {
        
            var auth = document.querySelector("p.auth-message");
            if (auth !== null)
                auth.remove();

            var mes = document.createElement("p");
            mes.classList.add("auth-message");
            mes.innerHTML = message;
            var loginForm = document.getElementById("register-form");
            if (loginForm !== null) {
                loginForm.appendChild(mes);
            }
        },

        printMessage: function (message) {
            var oldMes = document.querySelector("p.auth-message");
            if (oldMes !== null)
                oldMes.remove();

            var mes = document.createElement("p");
            mes.classList.add("auth-message");
            mes.innerHTML = message;
            var loginForm = document.getElementById("login-form");
            if (loginForm !== null) {
                loginForm.appendChild(mes);
            }
        },

        checkLogin: function () {
            var login = document.getElementById("login").value;

            if (login.length < 6) {
                self.printMessageReg("Login must be a least 6 symbols");
                document.getElementById("login").classList.add("invalid");
                return false;
            }
            else {
                document.getElementById("login").classList.add("valid");
                return true;
            }
        },

        checkName: function () {
            var name = document.getElementById("login-name");

            if (name.value.length < 3) {
                self.printMessageReg("Name length must be at least 3 symbols");
                name.classList.add("invalid");
                return false;
            }
            else {
                name.classList.add("valid");
                return true;
            }
        },

        checkPassword: function () {
            var password = document.getElementById("password");
            var passwordRepeat = document.getElementById("password-repeat");

            if (password.value.length < 6) {
                self.printMessageReg("Passwords length must be at least 6 symbols");
                password.classList.add("invalid");
                passwordRepeat.classList.add("invalid");
                return false;
            }

            if (password.value !== passwordRepeat.value) {
                self.printMessageReg("Passwords must be the same");
                password.classList.add("invalid");
                passwordRepeat.classList.add("invalid");
                return false;
            }
            else {
                password.classList.add("valid");
                passwordRepeat.classList.add("valid");
                return true;
            }
        },

        checkEmail: function () {
            var email = document.getElementById("login-email");
            var re = /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|ua|ru|net|gov|mil|biz|info|mobi|name|aero|jobs|museum)\b/;

            if (re.test(email.value)) {
                email.classList.add("valid");
                return true;
            }
            else {
                self.printMessageReg("Wrong email");
                email.classList.add("invalid");
                return false;
            }
        },

        showUser: function (user){
            var userInfo = document.createElement("p");
            userInfo.id = "user-info";
            userInfo.innerHTML = "Howdy, " + user + "!";

            var content = document.getElementById("authentification");
            content.appendChild(userInfo);

            self.deleteBackground(document.querySelector(".modal-background"));

            var btn = document.getElementById("login-btn");
            btn.style.display = "none";

            if (document.getElementById("logout-btn") === null) {
                var btnLogout = document.createElement("button");
                btnLogout.id = "logout-btn";
                btnLogout.innerHTML = "Logout"

                var elem = btn.parentElement;
                elem.insertBefore(btnLogout, btn);

                btnLogout.addEventListener("click", self.LogOut);
            }
            else {
                document.getElementById("logout-btn").style.display = "inline-block";
            }
        },

        LogOut: function () {
            var info = document.getElementById("user-info");
            if (info !== null)
                info.remove();
            document.getElementById("login-btn").style.display = "inline-block";
            document.getElementById("logout-btn").style.display = "none"; 
        }

    }
}