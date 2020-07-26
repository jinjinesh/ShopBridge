<template>
  <div>
    <v-container>
      <v-img
        class="thumbnail"
        alt="Click to upload image"
        :src="imageSrc"
        @click="openFile"
      />
      <ValidationObserver
        ref="observer"
        v-slot="{ invalid }"
      >
        <ValidationProvider
          v-slot="{ errors }"
          name="Name"
          rules="required|alpha-numeric-spaces|max:100"
        >
          <v-text-field
            v-model="model.name"
            :counter="100"
            :error-messages="errors"
            label="Name"
            required
            clearable
          />
        </ValidationProvider>
        <ValidationProvider
          v-slot="{ errors }"
          name="Description"
          rules="required|max:500"
        >
          <v-text-field
            v-model="model.description"
            :counter="500"
            :error-messages="errors"
            label="Description"
            required
            clearable
          />
        </ValidationProvider>
        <ValidationProvider
          v-slot="{ errors }"
          name="Price"
          rules="required|positive"
        >
          <v-text-field
            v-model="model.price"
            :error-messages="errors"
            label="Price"
            required
            clearable
          />
        </ValidationProvider>
        <v-btn
          color="primary"
          :disabled="invalid"
          @click="onSave"
        >
          Save
        </v-btn>
      </ValidationObserver>
    </v-container>
    <input
      type="file"
      v-show="false"
      ref="imageUpload"
      accept="image/png, image/jpeg, image/bmp"
      @change="uploadImage"
    >
    <feedback-bar
      :is-visible.sync="isFeedbackBarVisible"
      :feedback-items="feedbackItems"
      :status-code="statusCode"
    />
  </div>
</template>

<script>

import http from '@/utilities/httpWrapper.js';
import validatorMixin from '@/mixins/validatorMixin.js';
import FeedbackBar from '@/components/FeedbackBar.vue';
import helper from '@/utilities/helper.js';

export default {
    name: 'ItemView',
    mixins: [
        validatorMixin
    ],
    components: {
        FeedbackBar
    },
    props: {
        id: {
            type: String,
            default: null
        }
    },
    data () {
        return {
            valid: true,
            defaultModel: {
                id: null,
                name: null,
                price: null,
                description: null,
                imageData: null
            },
            model: {
                id: null,
                name: null,
                price: null,
                description: null,
                imageData: null
            },
            isFeedbackBarVisible: false,
            feedbackItems: [],
            statusCode: 0
        };
    },
    mounted () {
        if (this.id) {
            this.getItemData(this.id);
        } else {
            this.model = JSON.parse(JSON.stringify(this.defaultModel));
        }
    },
    computed: {
        imageSrc () {
            if (this.model.imageData) {
                return `data:image/jpeg;base64, ${this.model.imageData}`;
            } else {
                return '/noImage.png';
            }
        },
        isDataValid () {
            return this.model && this.model.name && this.model.price && this.model.description;
        }
    },
    methods: {
        getItemData (id) {
            return http.get(`/api/item/${id}`)
                .then(response => {
                    this.model = response.data.data;
                });
        },
        uploadImage (fileData) {
            if (fileData && fileData.target && fileData.target.files && fileData.target.files.length > 0) {
                const reader = new FileReader();
                reader.readAsDataURL(fileData.target.files[0]);
                reader.onloadend = () => {
                    const data = reader.result;
                    this.model.imageData = data.split(',')[1];
                };
            }
        },
        openFile () {
            this.$refs.imageUpload.click();
        },
        onSave () {
            console.log(this.$refs.observer.validate());
            const isIdNullOrEmpty = helper.guidIsNullOrEmpty(this.model.id);
            const url = isIdNullOrEmpty ? 'api/item' : `/api/item/${this.model.id}`;
            const instance = isIdNullOrEmpty ? http.post : http.put;
            return instance(url, this.model)
                .then((response) => {
                    if (response.data.response.statusCode === 0) {
                        if (isIdNullOrEmpty) {
                            this.$router.push(`/item?id=${response.data.data.id}`);
                        }
                        this.model = response.data.data;
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

<style lang="less" scoped>
.thumbnail {
  width: 100px;
  height: 100px;
  margin-left: auto;
  border: 2px solid black;
}
.btn-upload-image {
  margin-left: auto;
}
</style>
