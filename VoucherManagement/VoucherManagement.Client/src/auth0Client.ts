import type { Auth0VueClient } from '@auth0/auth0-vue'

let auth0Client: Auth0VueClient | null = null

export function setAuth0Client(client: Auth0VueClient) {
  auth0Client = client
}

export function getAuth0Client() {
  return auth0Client
}