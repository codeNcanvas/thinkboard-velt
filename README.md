# Thinkboard

A full-stack idea management platform built with the MERN stack (MongoDB, Express, React, Node.js), with Upstash Redis powering API rate limiting.

## Tech Stack

**Frontend**
- React 19 + Vite
- Tailwind CSS + daisyUI
- React Router
- Axios
- React Hot Toast

**Backend**
- Node.js + Express
- MongoDB + Mongoose
- Upstash Redis (`@upstash/ratelimit`) for rate limiting

## Project Structure

```
thinkboard-velt/
├── backend/
│   ├── src/
│   │   ├── config/
│   │   │   ├── db.js          # MongoDB connection
│   │   │   └── upstash.js     # Redis rate limiter setup
│   │   ├── controllers/
│   │   │   └── notesController.js
│   │   ├── middleware/
│   │   │   └── rateLimiter.js
│   │   ├── models/
│   │   │   └── Note.js
│   │   ├── routes/
│   │   │   └── notesRoutes.js
│   │   └── server.js          # Express app entry point
│   ├── .env                   # not committed — see below
│   └── package.json
├── frontend/
│   ├── src/
│   └── package.json
└── package.json
```

## Prerequisites

- Node.js (v18+ recommended)
- A MongoDB Atlas cluster (or local MongoDB instance)
- An Upstash Redis database (free tier is fine)

## Getting Started

### 1. Clone and install

```bash
git clone https://github.com/codeNcanvas/thinkboard-velt.git
cd thinkboard-velt

cd backend && npm install
cd ../frontend && npm install
```

### 2. Configure environment variables

Create `backend/.env` (this file is gitignored and never committed):

```env
MONGO_URI=mongodb+srv://<user>:<password>@<cluster>.mongodb.net/thinkboard
UPSTASH_REDIS_REST_URL=your-upstash-redis-url
UPSTASH_REDIS_REST_TOKEN=your-upstash-redis-token
PORT=5001
NODE_ENV=development
```

| Variable | Where to get it |
|---|---|
| `MONGO_URI` | MongoDB Atlas → Cluster → Connect → Drivers |
| `UPSTASH_REDIS_REST_URL` / `UPSTASH_REDIS_REST_TOKEN` | [console.upstash.com](https://console.upstash.com) → your Redis database → Details → REST API |
| `PORT` | Optional, defaults to `5001` |
| `NODE_ENV` | `development` locally, `production` when deployed |

The frontend needs no `.env` file — its API base URL is hardcoded based on Vite's mode (`http://localhost:5001/api` in dev, `/api` in production).

### 3. Run it

In one terminal:
```bash
cd backend
npm start
```
You should see:
```
MONGODB CONNECTED SUCCESSFULLY
Server started on PORT: 5001
```

In a second terminal:
```bash
cd frontend
npm run dev
```

Open **http://localhost:5173** in your browser.

## API Endpoints

Base path: `/api/notes`

| Method | Path | Description |
|---|---|---|
| GET | `/` | Get all notes (newest first) |
| GET | `/:id` | Get a single note by ID |
| POST | `/` | Create a note (`{ title, content }`) |
| PUT | `/:id` | Update a note (`{ title, content }`) |
| DELETE | `/:id` | Delete a note |

All requests pass through a rate limiter (100 requests / 60s window via Upstash).

## Available Scripts

**Root**
- `npm run build` — installs backend + frontend deps, builds the frontend for production
- `npm start` — runs the backend in production mode (serves the built frontend too)

**Backend** (`cd backend`)
- `npm start` — starts the Express server

**Frontend** (`cd frontend`)
- `npm run dev` — starts the Vite dev server
- `npm run build` — builds for production
- `npm run preview` — previews the production build
- `npm run lint` — runs ESLint

## Troubleshooting

- **`MODULE_NOT_FOUND` on `npm start`** — check that `backend/package.json`'s `start` script points to `node src/server.js`, not `node server.js`.
- **Mongo connection fails** — verify `MONGO_URI` has the correct password (no unencoded special characters like `@` or `#`) and that your current IP is allowed in Atlas → Network Access.
- **Rate limiter crashes on startup** — your Upstash database may have been auto-deleted for inactivity (common on the free tier). Create a new one and update the two `UPSTASH_*` values in `.env`.

## License

ISC
