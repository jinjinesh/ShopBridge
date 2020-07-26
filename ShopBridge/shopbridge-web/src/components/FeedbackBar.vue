<template>
  <v-snackbar
    v-model="isVisible"
    :color="color"
    :right="true"
    :multi-line="true"
    :timeout="timeout"
    :top="true"
  >
    <ul>
      <li
        v-for="(feedbackItem, index) in feedbackItems"
        :key="index"
      >
        <p>{{ feedbackItem.message }}</p>
      </li>
    </ul>
    <template v-slot:action="{ attrs }">
      <v-btn
        dark
        text
        v-bind="attrs"
        @click="isModalVisible = false"
      >
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script>
export default {
    name: 'FeedbackBar',
    props: {
        isVisible: {
            type: Boolean,
            default: false
        },
        timeout: {
            type: Number,
            default: 30000
        },
        feedbackItems: {
            type: Array,
            default: () => []
        },
        statusCode: {
            type: Number,
            default: 0
        }
    },
    computed: {
        color () {
            switch (this.statusCode) {
            case 0:
                return 'success';
            case 1:
                return 'info';
            case 2:
                return 'error';
            default:
                return 'info';
            }
        },
        isModalVisible: {
            get () {
                return this.isVisible;
            },
            set (value) {
                this.$emit('update:isVisible', value);
            }
        }
    }
};
</script>
