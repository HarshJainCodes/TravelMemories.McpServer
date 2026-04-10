<template>
	<login-page-v2
		website-name="Travel Memories"
		:google-client-id="googleClientId"
		mini-title="Travel Diary"
		@on-google-authenticated="onGoogleAuthenticated"
		@on-send-verification-code="onSendVerficationCode"
		@on-click-verify-otp="onClickVerifyOtp"
		@on-click-resend-otp="onClickResendOtp"
	/>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { LoginPageV2, LoginModes } from 'corecomponentshj';
import { BACKEND_URL } from './Constants';
import { TYPE, useToast } from 'vue-toastification';
import type { CallbackTypes } from 'vue3-google-login';

export default defineComponent({
	components: {
		LoginPageV2,
	},
	props: {
		loginChallenge: {
			type: String,
			required: true,
		},
	},
	emits: ['authenticated'],
	setup(props, { emit }) {
		const toast = useToast();
		const googleClientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;

		const onGoogleAuthenticated: CallbackTypes.TokenResponseCallback = async (res) => {
			console.log('here');
			const google_jwt = res.access_token;

			const googleLoginCall = await fetch(`${BACKEND_URL}/auth/googleLogin`, {
				method: 'POST',
				headers: {
					'Content-type': 'application/json; charset=UTF-8',
				},
				body: JSON.stringify({
					idToken: google_jwt,
					loginChallenge: props.loginChallenge,
					needOAuthCode: true,
				}),
				credentials: 'include',
			});

			if (googleLoginCall.status === 200) {
				const response = await googleLoginCall.json();

				emit('authenticated', {
					code: response.code,
					redirectUri: response.redirectUri,
				});
			}
			if (googleLoginCall.status === 401) {
				toast('Not able to authenticate', {
					type: TYPE.ERROR,
				});
			}
		};

		const onSendVerficationCode = async (email: String) => {
			const sendVerficationCall = await fetch(
				`${BACKEND_URL}/EmailService/SendOtpOAuthFlow/`,
				{
					method: 'POST',
					body: JSON.stringify({
						email,
						loginChallenge: props.loginChallenge,
					}),
					headers: {
						'Content-type': 'application/json; charset=UTF-8',
					},
				},
			);

			if (sendVerficationCall.status === 200) {
				toast('OTP Sent Successfully!', {
					type: TYPE.SUCCESS,
				});
			}
		};

		const onClickResendOtp = async (enteredEmail: string) => {
			await onSendVerficationCode(enteredEmail);
		};

		const onClickVerifyOtp = async ({
			enteredEmail,
			enteredOtp,
		}: {
			enteredEmail: string;
			enteredOtp: string;
		}) => {
			const verifyOtpRequest = await fetch(`${BACKEND_URL}/EmailService/VerifyOtpVsCode`, {
				method: 'POST',
				body: JSON.stringify({
					email: enteredEmail,
					otp: enteredOtp,
				}),
				headers: {
					'Content-type': 'application/json; charset=UTF-8',
				},
				credentials: 'include',
			});

			if (verifyOtpRequest.status === 200) {
				const responseData = await verifyOtpRequest.json();
				emit('authenticated', responseData);
			} else {
				toast('Invalid or expired OTP', {
					type: TYPE.ERROR,
				});
			}
		};

		return {
			googleClientId,
			onSendVerficationCode,
			onClickResendOtp,
			onClickVerifyOtp,
			onGoogleAuthenticated,
		};
	},
});
</script>
