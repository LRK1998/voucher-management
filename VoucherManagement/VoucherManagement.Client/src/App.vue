<script setup>
import Navbar from '@/components/Navbar.vue'
import { RouterView } from 'vue-router'
</script>

<template>
  <div class="flex flex-col min-h-screen">
    <!-- Navbar -->
    <Navbar />
    <!-- Main Content -->
    <main class="max-w-7xl container mx-auto p-4">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
/* Basis-Layout */
html, body, #app {
  margin: 0;
  padding: 0;
  height: 100%;
}

.flex {
  display: flex;
}

.flex-col {
  flex-direction: column;
}

.flex-1 {
  flex: 1;
}

.min-h-screen {
  min-height: 100vh;
}

p-4 {
  padding: 1rem;
}
</style>