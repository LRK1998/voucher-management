<template>
  <div class="max-w-sm mx-auto mt-20 space-y-6">
    <h2 class="text-2xl font-bold text-center">Login</h2>

    <div v-if="error" class="text-red-600 text-sm">{{ error }}</div>

    <form @submit.prevent="submit" class="space-y-4">
      <div class="space-y-2">
        <Label for="username">Benutzername</Label>
        <Input id="username" v-model="username" />
      </div>

      <div class="space-y-2">
        <Label for="password">Passwort</Label>
        <Input id="password" type="password" v-model="password" />
      </div>

      <Button class="w-full" type="submit">Login</Button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

const username = ref<string>('')
const password = ref<string>('')
const error = ref<string | null>(null)
const auth = useAuthStore()
const router = useRouter()

async function submit(): Promise<void> {
  error.value = null
  try {
    await auth.login(username.value, password.value)
    router.push('/')
  } catch {
    error.value = 'Login fehlgeschlagen'
  }
}
</script>

