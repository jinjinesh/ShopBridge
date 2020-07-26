import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '../views/Home.vue';
import ItemView from '../views/ItemView.vue';

Vue.use(VueRouter);

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/item',
        name: 'ItemView',
        component: ItemView,
        props: (route) => ({ id: route.query.id })
    }
];

const router = new VueRouter({
    routes
});

export default router;
