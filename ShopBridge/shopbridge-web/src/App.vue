<template>
  <v-app>
    <navigartion-bar />
    <v-main transition="slide-x-transition">
      <v-container fluid>
        <v-row
          align="center"
          justify="center"
        >
          <v-col class="text-center">
            <router-view :key="$route.fullPath" />
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>

import NavigartionBar from '@/components/NavigationBar';

export default {
    name: 'App',
    components: {
        NavigartionBar
    }
};
</script>
