import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';

// vuetify

import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import '@mdi/font/css/materialdesignicons.css'; // Ensure this is imported

// toasts
import Toast from 'vue-toastification';
import 'vue-toastification/dist/index.css';

import { aliases, mdi } from 'vuetify/iconsets/mdi';
import router from './router';

const app = createApp(App);

const vuetify = createVuetify({
	components,
	directives,
	icons: {
		defaultSet: 'mdi',
		aliases,
		sets: {
			mdi,
		},
	},
	theme: {
		defaultTheme: 'light',
	},
});

app.use(createPinia());
app.use(router);
app.use(vuetify);
app.use(Toast);
app.mount('#app');
