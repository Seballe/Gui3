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
    import axios from 'axios';

    export default
        {
            data() {
                return {
                    login: {
                        email: "",
                        password:""
                    }
                }
            }
            methods: {
                loggingIn: function () {
                    var ref = this;

                    axios.post('http://localhost:5000/api/AccountController/login',
                        {
                            Email: ref.login.email,
                            Password: ref.login.password
                        })
                    this.submitted = true;
                }
            },


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
