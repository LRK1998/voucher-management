import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import { useAuth0 } from '@auth0/auth0-vue'
import { authGuard } from '@auth0/auth0-vue'
import HomeView from '@/views/HomeView.vue'


const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: HomeView,
    beforeEnter: authGuard,
    meta: { requiresAuth: true }
  }
]
const router = createRouter({  
  history: createWebHistory(),
  routes,
})

router.beforeEach(async (to) => {
  const { isAuthenticated, isLoading, loginWithRedirect } = useAuth0()

  // ⏳ Auth0 lädt → Navigation erlauben
  if (isLoading.value) {
    return true
  }

  // 🔐 Geschützte Route
  if (to.meta.requiresAuth && !isAuthenticated.value) {
    await loginWithRedirect({
      appState: { target: to.fullPath }
    })
    return false
  }

  return true
})
export default router