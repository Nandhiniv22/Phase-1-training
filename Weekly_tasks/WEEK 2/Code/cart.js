const cartContainer = document.getElementById("cartContainer");
const query = new URLSearchParams(window.location.search);

const user = localStorage.getItem("loggedInUser");
const cartKey = `cart_${user}`;
let cart = [];

const storedCart = JSON.parse(localStorage.getItem(cartKey) || "[]");

if (query.has("data")) {
  try {
    cart = JSON.parse(decodeURIComponent(query.get("data")));
    localStorage.setItem(cartKey, JSON.stringify(cart)); 
  } catch (e) {
    alert("Invalid cart data");
  }
} else {
  cart = storedCart;
}

cart.forEach(item => {
  if (!item.quantity) item.quantity = 1;
});
localStorage.setItem(cartKey, JSON.stringify(cart));

function renderCart() {
  cartContainer.innerHTML = "";
  if (cart.length === 0) {
    cartContainer.innerHTML = `<div class="alert alert-info">Your cart is empty.</div>`;
    updateCartCount();
    return;
  }

  let total = 0;
  cart.forEach((item, index) => {
    total += item.price * item.quantity;

    const div = document.createElement("div");
    div.className = "cart-item";
    div.innerHTML = `
      <img src="${item.image}" alt="Food Image">
      <div class="cart-details">
        <h5>${item.name}</h5>
        <p>₹${item.price} x ${item.quantity} = ₹${item.price * item.quantity}</p>
        <p><strong>${item.category}</strong></p>
        <div>
          <button class="btn btn-secondary btn-sm me-1" onclick="decreaseQty(${index})">−</button>
          <button class="btn btn-secondary btn-sm" onclick="increaseQty(${index})">+</button>
        </div>
      </div>
      <button class="btn btn-danger btn-sm ms-3" onclick="removeItem(${index})">🗑 Delete</button>
    `;
    cartContainer.appendChild(div);
  });

  const totalDiv = document.createElement("div");
  totalDiv.className = "mt-4";
  totalDiv.innerHTML = `
    <h4>Total: ₹${total}</h4>
    <a href="payment.html?data=${encodeURIComponent(JSON.stringify(cart))}" class="btn btn-success mt-2">🛍️ Order Now</a>
  `;
  cartContainer.appendChild(totalDiv);

  updateCartCount();
}

function increaseQty(index) {
  cart[index].quantity++;
  localStorage.setItem(cartKey, JSON.stringify(cart));
  renderCart();
}

function decreaseQty(index) {
  if (cart[index].quantity > 1) {
    cart[index].quantity--;
  } else {
    cart.splice(index, 1);
  }
  localStorage.setItem(cartKey, JSON.stringify(cart));
  renderCart();
}

function removeItem(index) {
  cart.splice(index, 1);
  localStorage.setItem(cartKey, JSON.stringify(cart));
  renderCart();
}

function updateCartCount() {
  const currentCart = JSON.parse(localStorage.getItem(cartKey)) || [];
  document.getElementById("cartCount").textContent = currentCart.length;
}

function logout() {
    localStorage.removeItem('loggedInUser');
    window.location.href = 'login.html';
  }

renderCart();
