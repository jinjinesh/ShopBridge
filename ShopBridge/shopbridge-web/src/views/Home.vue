<template>
  <v-card>
    <v-card-title>
      <span>
        <span>Item</span>
        <span />
        <v-btn
          class="sm-1"
          icon
          dark
          color="indigo"
          @click="addNewItem"
        >
          <v-icon dark>mdi-plus</v-icon>
        </v-btn>
      </span>
      <v-spacer />
      <v-text-field
        v-model="search"
        append-icon="mdi-magnify"
        label="Search"
        single-line
        hide-details
      />
    </v-card-title>
    <v-data-table
      :headers="headers"
      :items="items"
      :search="search"
      @dblclick:row="onRowClick"
    >
      <template v-slot:item.actions="{ item }">
        <v-btn
          small
          icon
          @click="deleteItem(item)"
        >
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table>
    <feedback-bar
      :is-visible.sync="isFeedbackBarVisible"
      :feedback-items="feedbackItems"
      :status-code="statusCode"
    />
  </v-card>
</template>

<script>

import http from '@/utilities/httpWrapper.js';
import FeedbackBar from '@/components/FeedbackBar.vue';

export default {
    name: 'Home',
    components: {
        FeedbackBar
    },
    data () {
        return {
            search: '',
            headers: [
                { text: 'Name', align: 'start', value: 'name' },
                { text: 'Description', value: 'description' },
                { text: 'Price', value: 'price' },
                { text: 'Actions', value: 'actions', sortable: false }
            ],
            items: [],
            isFeedbackBarVisible: false,
            feedbackItems: [],
            statusCode: 0
        };
    },
    mounted () {
        this.getAllItems();
    },
    methods: {
        getAllItems () {
            return http.get('/api/item')
                .then(response => {
                    this.items = response.data.data;
                });
        },
        addNewItem () {
            this.$router.push('/item');
        },
        onRowClick (_, data) {
            this.$router.push(`/item?id=${data.item.id}`);
        },
        deleteItem (item) {
            const index = this.items.indexOf(item);
            return http.delete(`/api/item/${item.id}`)
                .then(response => {
                    if (response && response.data.data) {
                        this.items.splice(index, 1);
                    }
                    this.handleApiOutcome(response.data.response);
                });
        },
        handleApiOutcome (response) {
            this.feedbackItems = response.feedback;
            this.statusCode = response.statusCode;
            this.isFeedbackBarVisible = true;
        }
    }
};
</script>
