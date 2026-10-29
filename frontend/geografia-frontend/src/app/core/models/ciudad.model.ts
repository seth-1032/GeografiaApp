
export interface Ciudad {
    ciudadId: number;
    departamentoId: number;
    nombre: string;
    activo: boolean;
}

export interface GuardarCiudad {
    departamentoId: number;
    nombre: string;
    activo: boolean;
}
