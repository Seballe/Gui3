<template>
    <div id="log-in">
        <h1>Log in</h1>
        <label>UserName</label>
        <input type="text" v-model.lazy="login.email" required />
        <label>Password</label>
        <input type="text" v-model.lazy="login.password" required />
        <button class="btn btn-secondary" @click.prevent="loggingIn"> Log in </button>
    </div>
</template>

<script>
    export default{
            data() {
                return {
                    login: {
                        email: "",
                        password:""
                    }
                }
        },

         methods:{
                login() {
                        fetch('https://localhost:5000/api/Account/login', {
                            method: 'POST',
                            body: JSON.stringify({
                                email: this.login.email,
                                password: this.login.password
                            }),
                            headers: new Headers({
                                'Content-Type': 'application/json'
                            })
                        }).then(res => {
                             
                            if (!res.ok) {
                                throw new Error('Error Occured');
                            }

                            res.json().then((token) => {

                                if (res.status)
                                    localStorage.setItem("token", token.jwt);
                            })

                        
                        })
                    }
                }
            }
</script>

<style>
    #log-in * {
        box-sizing: border-box;
    }

    #log-in {
        max-width: 800px;
        min-height: 1000px;
        margin: 0 auto;
    }

    label {
        display: block;
        margin: 20px 0 5px;
    }

    input[type="text"], textarea {
        display: block;
        width: 100%;
        padding: 8px;
    }
</style>
