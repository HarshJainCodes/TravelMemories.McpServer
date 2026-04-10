<template>
	<component
		:is="currentComponent"
		@on-click-allow="onClickAllow"
		@authenticated="onAuthenticated"
		:login-challenge="loginChallenge"
	/>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from 'vue';
import { useRoute } from 'vue-router';
import LoginPage from './LoginPage.vue';
import ConsentFrontPage from './ConsentFrontPage.vue';

export default defineComponent({
	components: {
		LoginPage,
		ConsentFrontPage,
	},
	setup() {
		const route = useRoute();
		const loginChallenge = computed(() => route.query.code_challenge as string);
		const state = computed(() => route.query.state as string);
		const clientId = computed(() => route.query.client_id as string);

		const currentState = ref('mainPage');

		const currentComponent = computed(() => {
			switch (currentState.value) {
				case 'mainPage':
					return ConsentFrontPage;
				case 'loginPage':
					return LoginPage;
				default:
					return ConsentFrontPage;
			}
		});

		const onClickAllow = () => {
			currentState.value = 'loginPage';
		};

		const onAuthenticated = (response: { code: string; redirectUri: string }) => {
			if (response.redirectUri) {
				const redirectURL = `${response.redirectUri}?state=${state.value}&clientId=${clientId.value}&code=${response.code}`;
				window.location.href = redirectURL;
			}
		};

		return {
			currentComponent,
			loginChallenge,
			onClickAllow,
			onAuthenticated,
		};
	},
});
</script>
