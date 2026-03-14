// src/api.ts
import axios from 'axios'
import { getAuth0Client } from '../auth0Client'

const api = axios.create({
  baseURL: import.meta.env.DEV
    ? 'http://localhost:5245/api'
    : '/api',
})

// Token automatisch anhängen
api.interceptors.request.use(async (config) => {
  const auth0 = getAuth0Client()

  if (auth0) {
    const token = await auth0.getAccessTokenSilently()
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

export default api