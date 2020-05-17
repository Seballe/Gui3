<template>
    <div id="add-job">
        <h1>Add a new Job</h1>
        <form v-if="!submitted">
            <label>Customer Name</label>
            <input type="text" v-model.lazy="job.customer" required />
            <label>Start date:</label>
            <date-pick v-model="startDate" :format="format"
                       :parseDate="parseDate" :formatDate="formatDate"
                       :inputAttributes="{size: 32}"
                       ></date-pick>
            <label>End date:</label>
            <select v-model="endDate" required>
                <option v-for="day in endDates" :key="day">{{day}}</option>
            </select>
            <label>Location</label>
            <input type="text" v-model.lazy="job.location" required />
            <label>Comment</label>
            <input type="text" v-model.lazy="job.comments" required />

            <button class="btn btn-secondary" @click.prevent="PostJob">Add Manager</button>
        </form>

        <div v-show="submitted">
            <h3>You have added a new manager</h3>
        </div>
        <div id="preview">
            <h3>Preview job</h3>
            <p>Customer {{ job.customer }}</p>
            <p>Location {{ job.location }}</p>
            <p>Comments {{ job.comments }}</p>
        </div>
    </div>
</template>

<script>

    import axios from 'axios';
    import DatePick from 'vue-date-pick';
    import fecha from 'fecha';

export default {

    data(){
    return{
        job:{
            customer: "",
            days: "",
            location:"",
            comments:""
        },
        endDates: [1, 2, 7, 14, 28],
        format: 'dddd MMMM do, YYYY',
        startDate: fecha.format(new Date(), 'dddd MMMM Do, YYYY'),
        submitted: false
    }
    },
    methods:{
        PostJob: function () {
            this.calcEndDate();
            axios.post('http://localhost:5000/api/Managers/PostManager',
                {
                    Customer: this.customer,
                    StartDate: this.startDate,
                    Days: this.days,
                    Location: this.Location,
                    Comments: this.comments
                })
            this.submitted = true;
        },
        calcEndDate() {
            const moment = this.$moment().add(this.endDate, 'days').format();
            window.console.log(this.endDate);
            this.job.days = moment;
        },
        parseDate(dateString, format) {
            return fecha.parse(dateString, format);
        },
        formatDate(dateObj, format) {
            return fecha.format(dateObj, format);
        }
    },
}
</script>

<style>
    #add-job * {
        box-sizing: border-box;
    }

    #add-job {
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
