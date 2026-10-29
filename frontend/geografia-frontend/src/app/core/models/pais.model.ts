
export interface Pais {
  paisId: number;
  nombre: string;
  codigoIso2: string | null;
  activo: boolean;
}

export interface GuardarPais {
  nombre: string;
  codigoIso2: string | null;
  activo: boolean;
}
