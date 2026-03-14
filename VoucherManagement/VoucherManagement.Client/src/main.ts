import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'
import { createAuth0 } from '@auth0/auth0-vue'
import { setAuth0Client } from './auth0Client'

const pinia = createPinia()
const auth0 = 
    createAuth0({
  domain: import.meta.env.VITE_AUTH0_DOMAIN,
  clientId:  import.meta.env.VITE_AUTH0_CLIENT_ID,
  cacheLocation: 'localstorage',      // WICHTIG
  useRefreshTokens: true,             // WICHTIG
  authorizationParams: {
    redirect_uri: window.location.origin,
    audience: import.meta.env.VITE_AUTH0_AUDIENCE,
    scope: import.meta.env.VITE_AUTH0_SCOPE
  }
})
const app = createApp(App)

app.use(auth0).use(router).use(pinia)

app.mount('#app')

setAuth0Client(app.config.globalProperties.$auth0)