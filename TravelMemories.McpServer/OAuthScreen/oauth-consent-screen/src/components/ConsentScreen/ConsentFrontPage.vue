<template>
	<div class="w-100 h-100 d-flex align-center justify-center">
		<v-card
			elevation="4"
			:min-width="300"
			:max-width="500"
			class="pa-5 d-flex flex-column justify-center align-center rounded-lg px-15"
		>
			<div class="text-display-small font-weight-bold text-teal-lighten-1 mt-10">
				Travel Diary
			</div>

			<div class="text-display-small font-weight-medium mt-10">Grant Permission</div>

			<div class="d-flex flex-column bg-blue-grey-lighten-5 pa-4 mt-5 rounded-lg w-100">
				<div class="font-weight-bold">Travel Diary MCP</div>

				<div class="text-title-small font-weight-thin">
					is requesting permission to access your Travel Diary account.
				</div>
			</div>

			<div class="d-flex flex-column align-start w-100 mt-5">
				<div class="text-title-medium font-weight-bold">
					The app is requesting permission to:
				</div>

				<div>
					<ul>
						<li>Access your name and email address.</li>
						<li>View your photos and trips</li>
						<li>Upload photos to your account</li>
						<li>Query your trip history</li>
					</ul>
				</div>
			</div>
			<v-btn color="teal-lighten-1" size="large" class="mt-5" @click="onClickAllow">
				Allow
			</v-btn>

			<div class="mt-7">
				By continuing, you agree to our
				<a href="#" class="text-teal-lighten-1">Terms of Service</a> and
				<a href="#" class="text-teal-lighten-1">Privacy Policy</a>.
			</div>
		</v-card>
	</div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
	name: 'ConsentFrontPage',
	emits: ['on-click-allow'],
	setup(props, { emit }) {
		const onClickAllow = () => {
			// Emit an event to the parent component to switch to the login page
			// You can also perform any additional logic here if needed
			emit('on-click-allow');
		};

		return {
			onClickAllow,
		};
	},
});
</script>
