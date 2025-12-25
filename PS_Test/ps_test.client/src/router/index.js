import { createRouter, createWebHistory } from 'vue-router'
import Products from '../components/products/Products.vue'

const routes = [
  {
    path: '/', //change to '/products'
    name: 'Products',
    component: Products
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
