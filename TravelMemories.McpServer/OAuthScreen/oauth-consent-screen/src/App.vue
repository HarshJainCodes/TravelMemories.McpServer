<script lang="ts">
import { defineComponent } from 'vue';
import { RouterView } from 'vue-router';

export default defineComponent({
	name: 'App',
	components: {
		RouterView,
	},
});
</script>

<template>
	<main>
		<v-app>
			<v-main>
				<div class="w-100 h-100 d-flex align-center justify-center">
					<router-view v-slot="{ Component }">
						<keep-alive>
							<component :is="Component" />
						</keep-alive>
					</router-view>
				</div>
			</v-main>
		</v-app>
	</main>
</template>

<style scoped></style>
