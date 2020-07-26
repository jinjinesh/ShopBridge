<template>
  <div>
    <v-container>
      <v-row>
        <v-col
          cols="12"
          md="4"
        >
          <v-text-field
            v-model="name"
          />
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script>
export default {
    name: 'ItemView'
};
</script>
