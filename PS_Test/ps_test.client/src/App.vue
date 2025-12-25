<template>
  <div class="app-container">
    <header class="top-nav">
      <nav>
        <router-link to="/">Товары</router-link>
      </nav>
    </header>

    <main class="content">
      <router-view />
    </main>
  </div>
</template>

<style scoped>
  .app-container {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    width: 100%;
  }

  .top-nav {
    background-color: #f8f9fa;
    padding: 20px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  }

  nav {
    display: flex;
/*    gap: 20px;*/
    justify-content: center;
  }

  .content {
    padding: 40px;
    max-width: 1200px;
    margin: 0 auto;
    width: 100%;
  }

  nav a {
    text-decoration: none;
    color: #2c3e50;
    font-weight: bold;
  }

  nav a.router-link-exact-active {
    color: #42b983;
  }
</style>
