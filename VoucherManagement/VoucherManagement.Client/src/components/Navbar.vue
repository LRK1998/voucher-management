<template>
  <nav class="border-b px-4 py-2">
    <div class="max-w-7xl mx-auto flex justify-between items-center"> 
      <!-- Logo -->
      <div class="flex space-x-2" @click="$router.push('/') " style="cursor: pointer;">
        <img :src="logoUrl" alt="Logo" class="w-18" />
        <span class="text-2xl pl-2">Gutschein-Management</span>
      </div>
      <!-- Navigation Menu -->
      <NavigationMenu>
        <NavigationMenuList>
         <!-- <NavigationMenuItem>
            <NavigationMenuTrigger>Item One</NavigationMenuTrigger>
            <NavigationMenuContent>
              <NavigationMenuLink>Link</NavigationMenuLink>
            </NavigationMenuContent>
          </NavigationMenuItem>-->
          <NavigationMenuItem>
            <Button v-if="!isAuthenticated" @click="loginWithRedirect()" variant="ghost">Login</Button>
            <Button v-else @click="handleLogout()" variant="ghost" >Logout</Button>
          </NavigationMenuItem>
        </NavigationMenuList>
      </NavigationMenu>
    </div>
  </nav>
</template>

<script setup lang="ts">
import {
  NavigationMenu,
  NavigationMenuItem,
   NavigationMenuList,
} from "@/components/ui/navigation-menu"
//import { useRoute } from 'vue-router'
import Button from "./ui/button/Button.vue";
import { useAuth0 } from '@auth0/auth0-vue'

//const auth = useAuthStore()
//const route = useRoute()
//const router = useRouter()
const logoUrl = '/api/branding/logo.png';
const { loginWithRedirect, logout, isAuthenticated, } = useAuth0()

function handleLogout() {
  logout({
    logoutParams: {
      returnTo: window.location.origin
    }
  })
}
</script>
