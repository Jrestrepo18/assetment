import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { authService } from '../../services/authService';
import { Mail, Lock, UserPlus, Eye, EyeOff } from 'lucide-react';

// Mapeo de errores de Identity a español
const errorTranslations = {
    'PasswordRequiresNonAlphanumeric': 'La contraseña debe tener al menos un carácter especial (!@#$%^&*).',
    'PasswordRequiresDigit': 'La contraseña debe tener al menos un número (0-9).',
    'PasswordRequiresUpper': 'La contraseña debe tener al menos una letra mayúscula (A-Z).',
    'PasswordRequiresLower': 'La contraseña debe tener al menos una letra minúscula (a-z).',
    'PasswordTooShort': 'La contraseña debe tener al menos 6 caracteres.',
    'DuplicateUserName': 'Este correo electrónico ya está registrado.',
    'DuplicateEmail': 'Este correo electrónico ya está registrado.',
    'InvalidEmail': 'El formato del correo electrónico no es válido.',
};

const translateError = (error) => {
    if (error.code && errorTranslations[error.code]) {
        return errorTranslations[error.code];
    }
    return error.description || 'Error desconocido. Por favor intente de nuevo.';
};

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (password !== confirmPassword) {
            setError('Las contraseñas no coinciden.');
            return;
        }

        if (password.length < 6) {
            setError('La contraseña debe tener al menos 6 caracteres.');
            return;
        }

        setLoading(true);
        try {
            await authService.register(email, password);
            navigate('/login');
        } catch (err) {
            if (err.response?.data?.errors && Array.isArray(err.response.data.errors)) {
                const translatedErrors = err.response.data.errors.map(translateError);
                setError(translatedErrors.join(' '));
            } else if (err.response?.data?.message) {
                setError(err.response.data.message);
            } else {
                setError('Error al registrar. Por favor intente de nuevo.');
            }
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-page">
            <div className="auth-container">
                <div className="auth-header">
                    <h1>Bienvenido a Aprende+</h1>
                    <p>Crea tu cuenta y empieza a enseñar</p>
                </div>

                <div className="auth-card">
                    <h2>Crear Cuenta</h2>

                    {error && (
                        <div className="alert alert-error">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label>Correo electrónico</label>
                            <div className="input-group">
                                <Mail className="input-icon" size={18} />
                                <input
                                    type="email"
                                    className="form-control with-icon"
                                    placeholder="ejemplo@correo.com"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>
                        </div>

                        <div className="form-group">
                            <label>Contraseña</label>
                            <div className="input-group">
                                <Lock className="input-icon" size={18} />
                                <input
                                    type={showPassword ? 'text' : 'password'}
                                    className="form-control with-icon"
                                    placeholder="Mínimo 6 caracteres"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                                <button
                                    type="button"
                                    className="btn-toggle-password"
                                    onClick={() => setShowPassword(!showPassword)}
                                    tabIndex={-1}
                                >
                                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                                </button>
                            </div>
                            <small className="form-hint">
                                Debe incluir mayúscula, número y carácter especial
                            </small>
                        </div>

                        <div className="form-group">
                            <label>Confirmar contraseña</label>
                            <div className="input-group">
                                <Lock className="input-icon" size={18} />
                                <input
                                    type={showConfirmPassword ? 'text' : 'password'}
                                    className="form-control with-icon"
                                    placeholder="Repite tu contraseña"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                    required
                                />
                                <button
                                    type="button"
                                    className="btn-toggle-password"
                                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                    tabIndex={-1}
                                >
                                    {showConfirmPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                                </button>
                            </div>
                        </div>

                        <button
                            type="submit"
                            className="btn btn-primary btn-block"
                            disabled={loading}
                        >
                            <UserPlus size={18} style={{ marginRight: '0.5rem' }} />
                            {loading ? 'Registrando...' : 'Crear Cuenta'}
                        </button>
                    </form>


                    <div className="auth-divider">
                        <span>¿Ya tienes cuenta?</span>
                    </div>

                    <Link to="/login" className="btn btn-outline btn-block">
                        Iniciar Sesión
                    </Link>
                </div>

                <p className="auth-footer">
                    © 2026 Aprende+. Todos los derechos reservados.
                </p>
            </div>
        </div>
    );
};

export default Register;
