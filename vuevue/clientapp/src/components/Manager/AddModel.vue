<template>
    <div id="add-manager">
        <h1>Add a new Manager</h1>
        <form v-if="!submitted">
            <label>First Name</label>
            <input type="text" v-model.lazy="model.firstName" required />
            <label>Last Name</label>
            <input type="text" v-model.lazy="model.lastName" required />
            <label>Email</label>
            <input type="text" v-model.lazy="model.email" required />
            <label>Phone Number</label>
            <input type="text" v-model.lazy="model.phoneNumber" required />


            <button class="btn btn-secondary" @click.prevent="PostManager">Add Manager</button>
        </form>

        <div v-show="submitted">
            <h3>You have added a new model</h3>
        </div>
        <div id="preview">
            <h3>Preview model</h3>
            <p>First name {{ model.firstName }}</p>
            <p>Last name {{ model.lastName }}</p>
            <p>Email {{ model.email }}</p>
            <p>Phone number {{ model.phoneNumber }}</p>
        </div>
    </div>
</template>

<script>

import axios from 'axios';
//import moment from 'vue-moment';

export default {

    data(){
    return{
        manager:{
            firstName:"",
            lastName: "",
            email:"",
            phoneNumber:""
        },
        submitted: false,


    }
    },
    methods:{
        PostManager: function () {
            this.calcEndDate();


            axios.post('http://localhost:5000/api/Models/PostModel',
                {
                    FirstName: this.firstName,
                    LastName: this.lastName,
                    Email: this.email,
                    PhoneNo: this.phoneNumber
                })
            this.submitted = true;
        }
    },
}
</script>

<style>
    #add-manager * {
        box-sizing: border-box;
    }

    #add-manager {
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

    #preview {
        padding: 10px 20px;
        border: 1px dotted #ccc;
        margin: 30px 0;
    }

    h3 {
        margin-top: 10px;
    }
</style>
