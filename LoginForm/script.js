const container = document.querySelector(".container"),
      pwShowHide = document.querySelectorAll(".showHidePw"),
      pwFields = document.querySelectorAll(".password"),
      signUp = document.querySelector(".signup-link"),
      login = document.querySelector(".login-link");

//   js code to show/hide password and change icon
pwShowHide.forEach(eyeIcon =>{
    eyeIcon.addEventListener("click", ()=>{
        pwFields.forEach(pwField =>{
            if(pwField.type ==="password"){
                pwField.type = "text";

                pwShowHide.forEach(icon =>{
                    icon.classList.replace("uil-eye-slash", "uil-eye");
                })
            }else{
                pwField.type = "password";

                pwShowHide.forEach(icon =>{
                    icon.classList.replace("uil-eye", "uil-eye-slash");
                })
            }
        }) 
    })
})

// js code to appear signup and login form
signUp.addEventListener("click", ( )=>{
    container.classList.add("active");
});
login.addEventListener("click", ( )=>{
    container.classList.remove("active");
});

class LoginUserDto {
    email;
    password;
} 

const loginButton = document.getElementById("loginButton");
loginButton.addEventListener("click", async e => {
    e.preventDefault();

    const loginUrl = "http://localhost:5103/login";
    const loginEmail = document.getElementById("loginEmail");
    const loginPassword = document.getElementById("loginPassword");

    let loginData = new LoginUserDto(); 
    loginData.email = loginEmail.value;
    loginData.password = loginPassword.value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", loginUrl, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.withCredentials = true;
    xhr.body = JSON.stringify(loginData);
    xhr.send(JSON.stringify(loginData));
    
    alert(xhr.body);
    alert(xhr.responseText);
});

class RegisterUserDto
{
    UserType;
    Email;
    Password;
    Phone;
    FirstName;
    LastName;
}


const registerButton = document.getElementById("registerButton");
registerButton.addEventListener("click", async e => {
    e.preventDefault();

    const registerName = document.getElementById("registerName");
    const registerEmail = document.getElementById("registerEmail");
    const registerPasswordOne = document.getElementById("registerPasswordOne");
    //const registerPasswordTwo = document.getElementById("registerPasswordTwo");
    const registerUrl = "http://localhost:5103/register";

    let RegisterUser = new RegisterUserDto(); 
    RegisterUser.UserType = 1;
    RegisterUser.Email = registerEmail.value;
    RegisterUser.Password = registerPasswordOne.value;
    RegisterUser.FirstName = registerName.value;
    RegisterUser.LastName = registerName.value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", registerUrl, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.withCredentials = true;
    xhr.body = JSON.stringify(RegisterUser);
    xhr.send(JSON.stringify(RegisterUser));
    
    alert(xhr.body);
    alert(xhr.responseText);
});