import { createRouter, createWebHistory } from 'vue-router';
import ConsentScreen from '../components/ConsentScreen/ConsentScreen.vue';

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{
			path: '/',
			name: 'ConsentScreen',
			component: ConsentScreen,
		},
	],
});

export default router;
