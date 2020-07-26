<template>
  <div>
    <v-navigation-drawer
      app
      v-model="drawer"
      class="indigo"
      dark
    >
      <v-list>
        <template v-for="(item, index) in items">
          <v-list-item
            :key="index"
            :to="item.url"
          >
            <v-list-item-action>
              <v-icon>{{ item.image }}</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              {{ item.title }}
            </v-list-item-content>
          </v-list-item>
          <v-divider :key="`divider-${index}`" />
        </template>
      </v-list>
    </v-navigation-drawer>
    <v-app-bar
      app
      color="indigo"
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>ShopBridge</v-toolbar-title>
    </v-app-bar>
  </div>
</template>

<script>
export default {
    name: 'NavigationBar',
    data () {
        return {
            appTitle: 'ShopBridge',
            drawer: false,
            items: [
                { title: 'Home', image: 'mdi-home', url: '/' },
                { title: 'AddNew', image: 'mdi-plus', url: '/item' }
            ]
        };
    }
};
</script>

<style scoped>
a {
    color: white;
    text-decoration: none;
}
</style>
