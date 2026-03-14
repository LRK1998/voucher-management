import { defineStore } from 'pinia'
import api from '@/lib/api'

interface JwtPayload {
  role?: string
  name?: string
  [key: string]: unknown
}

function base64UrlDecode(str: string): string {
  str = str.replace(/-/g, '+').replace(/_/g, '/')
  while (str.length % 4) {
    str += '='
  }
  return atob(str)
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') as string | null,
    role: localStorage.getItem('role') as string | null,
    username: localStorage.getItem('username') as string | null,
  }),

  actions: {
    async login(username: string, password: string) {
      const res = await api.post('/users/login', { username, password })
      this.token = res.data.token

      if (this.token) {
        const base64Payload = this.token.split('.')[1]
        const jsonPayload = base64UrlDecode(base64Payload ?? '')
        const payload = JSON.parse(jsonPayload) as JwtPayload

        console.log(payload)

        this.role = payload.role || ''
        this.username = payload.name || 'Unbekannt'
      } else {
        this.role = ''
        this.username = ''
      }

      // Token und Userdaten speichern
      if (this.token) localStorage.setItem('token', this.token)
      if (this.role) localStorage.setItem('role', this.role)
      if (this.username) localStorage.setItem('username', this.username)
    },

    logout() {
      this.token = null
      this.role = null
      this.username = null

      localStorage.removeItem('token')
      localStorage.removeItem('role')
      localStorage.removeItem('username')
    },
  },
})
