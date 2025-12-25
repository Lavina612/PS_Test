<template>
  <div>
    <h1>Товары</h1>
    <div v-if="loading">Загрузка данных...</div>

    <div v-else-if="error">
      <p>Сервер еще запускается или недоступен.</p>
      <button @click="getProducts">Попробовать снова</button>
    </div>

    <div v-else-if="products.length === 0">
      <p>В базе данных пока нет ни одного товара.</p>
      <button @click="openAddDialog" class="btn-add">Добавить товар</button>
    </div>

    <div v-else>
      <button @click="openAddDialog" class="btn-add">Добавить товар</button>
      <table>
        <thead>
          <tr>
            <th>Код</th>
            <th>Название</th>
            <th>Цена</th>
            <th>Категория</th>
            <th>Действие</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in products" :key="product.id">
            <td>{{ product.code }}</td>
            <td>{{ product.name }}</td>
            <td>{{ product.price }}</td>
            <td>{{ product.category }}</td>
            <td>
              <div class="product-actions">
                <button @click="openUpdateDialog(product)">Изменить</button>
                <button @click="openDeleteDialog(product)">Удалить</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>

    <dialog ref="deleteDialog" class="modal">
      <div class="modal-content">
        <h3>Подтверждение удаления</h3>
        <p>Вы действительно хотите удалить товар <b>{{ deletingProduct?.code }}</b>?</p>
        <div class="modal-actions">
          <button @click="closeDeleteDialog">Отмена</button>
          <button @click="deleteProduct" class="btn-conf-delete">Удалить</button>
        </div>
      </div>
    </dialog>

    <dialog ref="addDialog" class="modal">
      <div class="modal-content">
        <h3>Добавление нового товара</h3>
        <form @submit.prevent="saveProduct">
          <div class="form-group">
            <label>Код:</label>
            <input v-model="newProduct.code" required placeholder="00-0000-AA00" pattern="\d{2}-\d{4}-[A-Z]{2}\d{2}" />
          </div>
          <div class="form-group">
            <label>Название:</label>
            <input v-model="newProduct.name" />
          </div>
          <div class="form-group">
            <label>Цена:</label>
            <input type="number" min=0 v-model.number="newProduct.price" />
          </div>
          <div class="form-group">
            <label>Категория:</label>
            <input v-model="newProduct.category" />
          </div>

          <div class="modal-actions">
            <button type="button" @click="addDialog.close()">Отмена</button>
            <button type="submit" class="btn-save">Сохранить</button>
          </div>
        </form>
      </div>
    </dialog>

    <dialog ref="updateDialog" class="modal">
      <div v-if="updatingProduct" class="modal-content">
        <h3>Редактирование товара</h3>
        <form @submit.prevent="updateProduct">
          <div class="form-group">
            <label>Код:</label>
            <input v-model="updatingProduct.code" required placeholder="00-0000-AA00" pattern="\d{2}-\d{4}-[A-Z]{2}\d{2}" />
          </div>
          <div class="form-group">
            <label>Название:</label>
            <input v-model="updatingProduct.name" />
          </div>
          <div class="form-group">
            <label>Цена:</label>
            <input type="number" min=0 v-model.number="updatingProduct.price" />
          </div>
          <div class="form-group">
            <label>Категория:</label>
            <input v-model="updatingProduct.category" />
          </div>

          <div class="modal-actions">
            <button type="button" @click="updateDialog.close()">Отмена</button>
            <button type="submit" class="btn-save">Сохранить изменения</button>
          </div>
        </form>
      </div>
    </dialog>
</template>

<script setup>
  import { ref, onMounted } from 'vue';

  const loading = ref(true);
  const error = ref(false);
  const products = ref([]);

  const deletingProduct = ref(null);
  const newProduct = ref({
    code: '',
    name: '',
    price: 0,
    category: ''
  });
  const updatingProduct = ref(null);

  const deleteDialog = ref(null);
  const addDialog = ref(null);
  const updateDialog = ref(null);

  const getProducts = async () => {
    loading.value = true;
    error.value = false;

    try {
      const response = await fetch('/api/products');
      if (!response.ok)
        throw new Error('Ошибка сети');

      const contentType = response.headers.get("content-type");
      if (!contentType || !contentType.includes("application/json")) {
         throw new Error('Сервер вернул HTML вместо JSON (сервер ещё не запустился)');
      }

      products.value = await response.json();
    } catch (err) {
      error.value = true;
      console.error("Ошибка при получении продуктов:", err);
    } finally {
      loading.value = false;
    }
  };

  const openDeleteDialog = (product) => {
    deletingProduct.value = product;
    deleteDialog.value.showModal();
  };

  const closeDeleteDialog = () => {
    deleteDialog.value.close();
    deletingProduct.value = null;
  };

  const deleteProduct = async () => {
    if (!deletingProduct.value) return;

    try {
      const response = await fetch(`/api/products/${deletingProduct.value.id}`, {
        method: 'DELETE'
      });

      if (response.ok) {
        products.value = products.value.filter(p => p.id !== deletingProduct.value.id);
        closeDeleteDialog();
      } else {
        alert('Ошибка при удалении на сервере');
      }
    } catch (error) {
      console.error("Ошибка запроса:", error);
      alert('Не удалось связаться с сервером');
    }
  };

  const openAddDialog = () => {
    newProduct.value = { code: '', name: '', price: 0, category: '' };
    addDialog.value.showModal();
  };

  const saveProduct = async () => {
    try {
      const response = await fetch('/api/products', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newProduct.value)
      });

      if (response.ok) {
        const createdProduct = await response.json();
        products.value.push(createdProduct);
        addDialog.value.close();
      } else {
        alert('Ошибка при сохранении');
      }
    } catch (error) {
      console.error("Ошибка:", error);
    }
  };

  const openUpdateDialog = (product) => {
    updatingProduct.value = { ...product };
    updateDialog.value.showModal();
  };

  const closeUpdateDialog = () => {
    updateDialog.value.close();
    updatingProduct.value = null;
  };

  // Функция отправки данных на бэкенд
  const updateProduct = async () => {
    try {
      const response = await fetch(`/api/products/${updatingProduct.value.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatingProduct.value)
      });

      if (response.ok) {
        const index = products.value.findIndex(p => p.id === updatingProduct.value.id);
        if (index !== -1) {
          products.value[index] = { ...updatingProduct.value };
        }
        closeUpdateDialog();
      } else {
        alert('Ошибка при обновлении');
      }
    } catch (error) {
      console.error("Ошибка:", error);
    }
  };

  onMounted(getProducts);
</script>

<style scoped>
  table {
    border-collapse: collapse;
  }

  th {
    background-color: #f1f1f1;
  }

  th, td {
    border: 1px solid #000000;
    padding: 5px 8px;
  }

  tr:hover {
    background-color: #f8f9fa;
  }

  button {
    background-color: #f1f1f1;
    padding: 3px 15px;
    border-radius: 2px;
    border: 1px solid #8c8c8c;
    cursor: pointer;
  }

  .product-actions {
    display: flex;
    justify-content: center;
    gap: 8px;
  }

  .modal {
    border: none;
    border-radius: 8px;
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    box-shadow: 0 10px 25px rgba(0,0,0,0.2);
  }

  .modal::backdrop {
    background: rgba(0, 0, 0, 0.5);
    backdrop-filter: blur(2px);
  }

  .modal-content {
    padding: 15px;
    min-width: 300px;
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 8px;
    margin-top: 20px;
  }

  .form-group {
    margin-bottom: 15px;
    display: flex;
    flex-direction: column;
    text-align: left;
  }

  .form-group label {
    font-weight: bold;
    margin-bottom: 2px;
  }

  .form-group input {
    padding: 8px;
    border: 1px solid #cccccc;
    border-radius: 4px;
  }

  .btn-add {
    margin-bottom: 10px;
  }

  .btn-save {
    background-color: #42b983;
    color: white;
    border: none;
  }


  .btn-conf-delete {
    background-color: #ff4d4f;
    color: white;
    border: none;
  }

  .btn-conf-delete:hover {
    background-color: #ff7875;
  }

</style>
