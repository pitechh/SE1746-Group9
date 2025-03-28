import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Login from "./pages/Login";
import ManageDelegates from "./pages/Admin/ManageDelegates";
import ManageConferences from "./pages/Admin/ManageConferences";
import Profile from "./pages/Profile";
import Home from "./pages/Home";
import Forbiden from "./pages/Forbiden";
import PrivateRoute from "./components/PrivateRoute";
import ManageConferenceDetails from "./pages/ConferenceHost/ManageConferenceHostingDetails";
import Delegates from "./pages/Delegates/Delegates";
import Conferences from "./pages/Delegates/Conferences";
import ConferenceHostingRegistration from "./pages/Delegates/ConferenceHostingRegistration";
import ConferenceHostingRegistrationList from "./pages/Delegates/ConferenceHostingRegistrationList";
import ConferenceDetails from "./pages/Delegates/ConferenceDetails";
import ManageDelegatesDetails from "./pages/Admin/ManageDelegatesDetails";
import AdminConferenceHostingRegistrationList from "./pages/Admin/AdminConferenceHostingRegistrationList";

function App() {
  return (
    <Router>
      <Navbar />
      <div className="p-4">
        <Routes>
          {/* Protected Admin Routes */}
          <Route element={<PrivateRoute />}>
            <Route path="/admin/delegates" element={<ManageDelegates />} />
            <Route path="/admin/conferences" element={<ManageConferences />} />
            <Route
              path="admin/conference/:id"
              element={<ManageConferenceDetails />}
            />
            <Route
              path="admin/conference-hosting-registration-list"
              element={<AdminConferenceHostingRegistrationList />}
            />
            <Route
              path="admin/delegate-details/:id"
              element={<ManageDelegatesDetails />}
            />
          </Route>

          <Route
            path="hosting/conference/:id"
            element={<ManageConferenceDetails />}
          />
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route
            path="/conference-hosting-registration-list"
            element={<ConferenceHostingRegistrationList />}
          />
          <Route
            path="conference-hosting-registration-form"
            element={<ConferenceHostingRegistration />}
          />
          <Route path="/delegates" element={<Delegates />} />
          <Route path="/conference/:id" element={<ConferenceDetails />} />

          <Route path="/conferences" element={<Conferences />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/forbiden" element={<Forbiden />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
