# 🔫 CS2 Plugin - ScopMetre

## 📌 Description

A customizable plugin for Counter-Strike 2 that announces no-scope kills in chat with multi-language support.

---

- ✅ Detects and announces no-scope (AWP/SSG08) kills in chat
- ✅ Multi-language support (auto-detects player's game language, 10+ languages included)
- ✅ Customizable chat prefix via config file
- ✅ Automatic config and language file creation
- ✅ Colorful, modern chat messages

---

## 🧩 Requirements

- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)

---

## 🛠️ Installation

### 1) Build or Download the Plugin

- Build with:
  ```
  dotnet build -c Release
  ```
  The DLL will be in `bin/Release/net8.0/`.

- Or download the latest release (if available).

### 2) Install the Plugin

Place the plugin files in the following directory on your server:

```
cs2/addons/counterstrikesharp/plugins/ScopMetre/
```

- Place the compiled `ScopMetre.dll` here.
- Place the `config` and `lang` folders here as well (or let the plugin create them automatically).

### 3) Start / Reload the Plugin

- Restart your server  
  **OR**
- Run the following command in the server console:
  ```
  css_plugins load ScopMetre
  ```

---

## ⚙️ Configuration

After the first run, a configuration file will be automatically generated at:

```
cs2/addons/counterstrikesharp/plugins/ScopMetre/config/ScopMetreConfig.json
```

- You can edit this file to change the chat prefix.

Language files are located in:

```
cs2/addons/counterstrikesharp/plugins/ScopMetre/lang/
```

- You can edit or add new language files to customize messages.

---

## 🌍 Supported Languages

- English, Turkish, German, French, Spanish, Russian, Italian, Polish, Portuguese, Chinese, Arabic

---

## 🤝 Contributing

Pull requests and issues are welcome!  
Feel free to add new languages or suggest improvements.

---

## 📄 License

MIT

---

**Enjoy your no-scope moments!** 
